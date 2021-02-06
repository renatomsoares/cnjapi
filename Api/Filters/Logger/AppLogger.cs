using System;
using Microsoft.Extensions.Logging;

namespace Application.Filters.Logger
{
    public class AppLogger : ILogger
    {
        private readonly string _nomeCategoria;
        private readonly Func<string, LogLevel, bool> _filtro;
        private readonly int _messageMaxLength = 4000;

        public AppLogger(string nomeCategoria, Func<string, LogLevel, bool> filtro, string connectionString)
        {
            _nomeCategoria = nomeCategoria;
            _filtro = filtro;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventoId,
            TState state, Exception exception, Func<TState, Exception, string> formato)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formato == null)
                throw new ArgumentNullException(nameof(formato));

            var mensagem = formato(state, exception);
            if (string.IsNullOrEmpty(mensagem))
            {
                return;
            }

            if (exception != null)
                mensagem += $"\n{exception}";
            

            mensagem = mensagem.Length > _messageMaxLength ? mensagem.Substring(0, _messageMaxLength) : mensagem;
            var eventLog = new ApplicationLog
            {
                Application = "CNJ API",
                Action = state.ToString(),
                Message = mensagem,
                LogLevel = logLevel.ToString(),
                EventDate = DateTime.Now
            };
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var retorno = (_filtro == null || _filtro(_nomeCategoria, logLevel));
            return retorno;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
