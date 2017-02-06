﻿using Microsoft.AspNet.Identity.EntityFramework;
using SchoolSystem.Data.Contracts;
using SchoolSystem.Data.Models;
using SchoolSystem.Data.Models.Common;
using SchoolSystem.Data.Models.CustomModels;
using SchoolSystem.Web.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace SchoolSystem.Web.Services
{
    public class NewsDataService : INewsDataService
    {
        private readonly IRepository<Newsfeed> newsfeedRepository;
        private readonly IRepository<User> userRepo;
        private readonly Func<IUnitOfWork> unitOfWork;

        public NewsDataService(
            IRepository<Newsfeed> newsfeedRepository,
            IRepository<User> userRepo,
            Func<IUnitOfWork> unitOfWork
            )
        {
            this.newsfeedRepository = newsfeedRepository;
            this.userRepo = userRepo;
            this.unitOfWork = unitOfWork;
        }

        public void AddNews(string username, string content, DateTime createdOn, bool isImportant)
        {
            using (var uow = this.unitOfWork())
            {
                var newsfeed = new Newsfeed();
                var user = this.userRepo.GetFirst(x => x.UserName == username);

                newsfeed.Content = content;
                newsfeed.CreatedOn = createdOn;
                newsfeed.IsImportant = isImportant;

                user.NewsFeed.Add(newsfeed);
                uow.Commit();
            }

        }

        /// <summary>
        ///  Returns the important news for the last 5 days
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NewsModel> GetImportantNews()
        {
            var last5Days = DateTime.Now.AddDays(-5);

            var result = this.newsfeedRepository.GetAll(
                x => x.IsImportant == true && x.CreatedOn > last5Days,
                x => new NewsModel()
                {
                    Creator = x.User.UserName,
                    AvatarPictureUrl = x.User.AvatarPictureUrl,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn
                })
                .OrderByDescending(x => x.CreatedOn);

            return result;
        }


        /// <summary>
        /// Returns the last "count" not important news (default value is 20)
        /// </summary>
        /// <param name="count">the number of news to return</param>
        /// <returns></returns>
        public IEnumerable<NewsModel> GetNews(int count = 20)
        {
            var result = this.newsfeedRepository.GetAll(
                x => x.IsImportant == false,
                x => new NewsModel()
                {
                    Creator = x.User.UserName,
                    AvatarPictureUrl = x.User.AvatarPictureUrl,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn
                })
                .OrderByDescending(x => x.CreatedOn)
                .Take(count);

            return result;
        }
    }
}