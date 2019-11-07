#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using Esprima;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime;

namespace DrawingPlayground {

    internal class JsRunner {

        private Engine engine;

        private readonly object engineLock;

        private readonly ILog log;

        public JsRunner(ILog log) {
            engineLock = new object();
            this.log = log;
            engine = MakeEngine();
        }

        private Engine MakeEngine() =>
            new Engine(cfg => cfg.AllowClr()
                                 .AllowClr(typeof(Brushes).Assembly)
                                 .Culture(CultureInfo.InvariantCulture)
                                 .CatchClrExceptions()
                                 .LimitMemory(1024L * 1024L * 1024L)
                                 .TimeoutInterval(TimeSpan.FromSeconds(1)))
                .Execute("var Drawing = importNamespace('System.Drawing');\n" +
                         "var Color = Drawing.Color;")
                .SetValue("console", new {
                    d = (Action<object?>)log.LogDebug,
                    i = (Action<object?>)log.LogInfo,
                    w = (Action<object?>)log.LogWarning,
                    e = (Action<object?>)log.LogError,
                    f = (Action<object?>)log.LogFatal,
                    log = (Action<object?>)log.LogInfo
                });

        public void Reset() {
            lock (engineLock) {
                engine = MakeEngine();
            }
        }

        public ParserException[] RunCode(string code) {
            var syntaxErrors = new List<ParserException>();
            var errorHandler = new ErrorHandler { Tolerant = true };
            var parser = new JavaScriptParser(code, new ParserOptions {
                SourceType = SourceType.Script,
                Tolerant = true,
                ErrorHandler = errorHandler
            });
            Esprima.Ast.Program? program = null;
            try {
                program = parser.ParseProgram();
            } catch (ParserException error) {
                syntaxErrors.Add(error);
            } catch (Exception e) {
                syntaxErrors.Add(
                    new ParserException(
                        new ParseError(
                            $"Parser error ({e.Message})",
                            errorHandler.Source,
                            0,
                            new Position(0, 0)
                        )
                    )
                );
            }
            syntaxErrors.AddRange(errorHandler.Errors);
            if (syntaxErrors.Count == 0) {
                try {
                    lock (engineLock) {
                        engine.ResetTimeoutTicks();
                        engine.Execute(program);
                    }
                } catch (JavaScriptException error) {
                    syntaxErrors.Add(
                        new ParserException(
                            new ParseError(
                                error.Message,
                                error.Location.Source,
                                GetIndex(code, error.LineNumber, error.Column),
                                error.Location.Start
                            )
                        )
                    );
                } catch (MemoryLimitExceededException error) {
                    log.LogError(error);
                } catch (TimeoutException error) {
                    log.LogError(error);
                } catch (RecursionDepthOverflowException error) {
                    log.LogError(error);
                }
            }
            return syntaxErrors.ToArray();
        }

        private int GetIndex(string code, int line, int column) {
            if (line == 0) {
                return column;
            }
            var matches = Regex.Matches(code, @"\n|\r\n?");
            var match = matches[Math.Min(matches.Count, line) - 1];
            return match.Index + match.Length + column;
        }

        public ObjectInstance? GetFunction(string name) {
            JsValue val;
            lock (engineLock) {
                val = engine.GetValue(name);
            }
            if (!val.IsObject()) {
                return null;
            }
            var obj = val.AsObject();
            return obj.Class == "Function" ? obj : null;
        }

        public void InvokeFunction(ObjectInstance func, object[] args) {
            if (func.Class != "Function") {
                throw new ArgumentException("Argument must be a JavaScript function", nameof(func));
            }
            try {
                lock (engineLock) {
                    engine.ResetTimeoutTicks();
                    engine.Invoke(func, args);
                }
            } catch (JavaScriptException error) {
                log.LogError(error);
            } catch (MemoryLimitExceededException error) {
                log.LogError(error);
            } catch (TimeoutException error) {
                log.LogError(error);
            } catch (RecursionDepthOverflowException error) {
                log.LogError(error);
            }
        }

    }

}
