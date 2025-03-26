using Contatos.DataContracts.Commands;
using Contatos.DeleteConsumer.Interfaces;
using MassTransit;

namespace Contatos.DeleteConsumer.Services
{
    public class ApagarContatoConsumer : IConsumer<ApagarContato>
    {
        private readonly IContatoRepository _repoContato;

        public ApagarContatoConsumer(IContatoRepository repoContato)
        {
            _repoContato = repoContato;
        }

        public async Task Consume(ConsumeContext<ApagarContato> context)
        {
            Console.WriteLine(context.Message);
            await _repoContato.ApagarContatoAsync(context.Message);
        }
    }
}
