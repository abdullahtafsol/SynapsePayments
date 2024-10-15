using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DocuSignHelper
{
    public interface IMemoryCacheHelper
    {
        public object Get(string key);
    }

    public class MemoryCacheHelper : IHostedService, IMemoryCacheHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheHelper(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Load();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public object Get(string key)
        {
            _memoryCache.TryGetValue(key, out object value);
            return value;
        }

        private void Load()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var stream = new FileStream($"{path}\\{_configuration["PrivateKeyPath"]}", FileMode.Open, FileAccess.Read, FileShare.None);

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                _memoryCache.Set("PrivateKey", memoryStream.ToArray());
            }
        }
    }
}
