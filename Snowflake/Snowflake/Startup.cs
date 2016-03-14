using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;
using Snowflake.Core.Domain;
using Snowflake.Core.Infrastructure;
using Snowflake.Core.Repository;
using Snowflake.Data.Infrastructure;
using Snowflake.Data.Repository;
using Snowflake.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Snowflake.Api.Startup))]
namespace Snowflake.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureSimpleInjector(app);
            ConfigureOAuth(app, container);

            HttpConfiguration config = new HttpConfiguration
            {
                DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container)
            };

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app, Container container)
        {
            Func<IAuthorizationRepository> authRepositoryFactory = container.GetInstance<IAuthorizationRepository>;

            var authorizationOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SnowflakeAuthorizationServerProvider(authRepositoryFactory)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(authorizationOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public Container ConfigureSimpleInjector(IAppBuilder app)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            container.Register<IDatabaseFactory, DatabaseFactory>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>();

            container.Register<IChoiceRepository, ChoiceRepository>();
            container.Register<IConversationRepository, ConversationRepository>();
            container.Register<IMessageRepository, MessageRepository>();
            container.Register<IParticipationRepository, ParticipationRepository>();
            container.Register<IThoughtRepository, ThoughtRepository>();
            container.Register<ISnowflakeUserRepository, SnowflakeUserRepository>();
            container.Register<IUserStore<SnowflakeUser, int>, UserStore>(Lifestyle.Scoped);
            container.Register<IAuthorizationRepository, AuthorizationRepository>(Lifestyle.Scoped);

            // more code to facilitate a scoped lifestyle
            app.Use(async (context, next) =>
            {
                using (container.BeginExecutionContextScope())
                {
                    await next();
                }
            });

            container.Verify();

            return container;
        }
    }
}
