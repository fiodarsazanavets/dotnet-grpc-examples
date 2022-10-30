using Grpc.Net.Client.Balancer;

namespace WebApiGrpcClient;

public class RandmomizedBalancer : SubchannelsLoadBalancer
{
    public RandmomizedBalancer(IChannelControlHelper controller, ILoggerFactory loggerFactory)
        : base(controller, loggerFactory)
    {
    }

    protected override SubchannelPicker CreatePicker(IReadOnlyList<Subchannel> readySubchannels)
    {
        return new RandomizedPicker(readySubchannels);
    }

    private class RandomizedPicker : SubchannelPicker
    {
        private readonly IReadOnlyList<Subchannel> _subchannels;

        public RandomizedPicker(IReadOnlyList<Subchannel> subchannels)
        {
            _subchannels = subchannels;
        }

        public override PickResult Pick(PickContext context)
        {
            return PickResult.ForSubchannel(_subchannels[Random.Shared.Next(0, _subchannels.Count)]);
        }
    }

    public class RandomizedBalancerFactory : LoadBalancerFactory
    {
        public override string Name => "randomized";

        public override LoadBalancer Create(LoadBalancerOptions options)
        {
            return new RandmomizedBalancer(options.Controller, options.LoggerFactory);
        }
    }
}
