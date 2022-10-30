using Grpc.Net.Client.Balancer;
using System.Collections.ObjectModel;
using System.Net;

namespace WebApiGrpcClient;

public class CsvResolver : Resolver
{
    private readonly Uri _address;
    private Action<ResolverResult> _listener;

    public CsvResolver(Uri address)
    {
        _address = address;
    }

    public override void Refresh()
    {
        var addresses = new List<BalancerAddress>();

        foreach (var line in File.ReadLines(_address.Host))
        {
            var addresComponents = line.Split(',');
            addresses.Add(new BalancerAddress(addresComponents[0], int.Parse(addresComponents[1])));
        }

        _listener(ResolverResult.ForResult(addresses));
    }

    public override void Start(Action<ResolverResult> listener)
    {
        _listener = listener;
    }
}

public class CsvResolverFactory : ResolverFactory
{
    public override string Name => "csv";

    public override Resolver Create(ResolverOptions options)
    {
        return new CsvResolver(options.Address);
    }
}
