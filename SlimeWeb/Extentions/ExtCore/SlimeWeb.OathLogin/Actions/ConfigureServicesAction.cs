﻿using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.OathLogin.Actions
{
	public class ConfigureServicesAction : IConfigureServicesAction
	{
		public int Priority => 1000;

		public void Execute(IServiceCollection services, IServiceProvider serviceProvider)
		{
			try
			{
   //             IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
			//	 .SetBasePath(serviceProvider.GetService<IWebHostEnvironment>().ContentRootPath)
			//	.AddJsonFile("SlimeWeb.OathLogin.json", optional: true, reloadOnChange: true);
                

   //             var builder = WebApplication.CreateBuilder();
   //             var config = builder.Configuration;
   //             builder.Services.AddAuthentication()
   //                            .AddGoogle(options =>
   //                         {
   //                             IConfigurationSection googleAuthNSection =
   //                             config.GetSection("Authentication:Google");
   //                             options.ClientId = googleAuthNSection["ClientId"];
   //                             options.ClientSecret = googleAuthNSection["ClientSecret"];
   //                         })
   //                         .AddFacebook(options =>
   //                         {
   //                             IConfigurationSection FBAuthNSection =
   //                             config.GetSection("Authentication:FB");
   //                             options.ClientId = FBAuthNSection["ClientId"];
   //                             options.ClientSecret = FBAuthNSection["ClientSecret"];
   //                         })
   //                         .AddMicrosoftAccount(microsoftOptions =>
   //                         {
   //                             microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
   //                             microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
   //                         })
   //                         .AddTwitter(twitterOptions =>
   //                         {
   //                             twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
   //                             twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
   //                             twitterOptions.RetrieveUserDetails = true;
   //                         });





            }
            catch (Exception ex)
			{
				CommonTools.ErrorReporting(ex);

			}
		}
	}
	}
