using CreditCard.Grpc.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace Prototype.Payment.Api.Grpc;

public class CreditCardGrpcClient(string grpcEndpoint) : CreditCardGrpcService.CreditCardGrpcServiceBase
{
    private readonly string _grpcEndpoint = grpcEndpoint;

    public override async Task<CreditCardResponse> CreateCreditCard(CreateCreditCardRequest request, ServerCallContext context)
    {
        // Cria um channel para chamar o serviço remoto
        // TODO: Importante cuidar para que não sejam abertos canais para cada request, talvez criar um singleton.
        using var channel = GrpcChannel.ForAddress(_grpcEndpoint);

        // Cria o client gerado pelo gRPC
        var remoteClient = new CreditCardGrpcService.CreditCardGrpcServiceClient(channel);

        // Faz a chamada assíncrona ao serviço remoto
        var response = await remoteClient.CreateCreditCardAsync(request);

        // Retorna a resposta obtida do serviço remoto
        return response;
    }
}
