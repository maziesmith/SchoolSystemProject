﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SchoolSystem.Data.Models.CustomModels;
using SchoolSystem.MVP.Home.Models;
using SchoolSystem.MVP.Home.Presenter;
using SchoolSystem.MVP.Home.Views;
using SchoolSystem.Web.Services.Contracts;

namespace SchoolSystem.MVP.Tests.Home.Presenters.NewsfeedPresenterTests
{
    [TestFixture]
    public class BindNewsFeedData_Should
    {
        [Test]
        public void Set_NewsFeed_And_ImportantNews_Correctly()
        {
            var mockedNewsfeedView = new Mock<INewsfeedView>();
            var mockedNewsDataService = new Mock<INewsDataService>();

            var newsDataPresenter = new NewsfeedPresenter(mockedNewsfeedView.Object, mockedNewsDataService.Object);

            var newsfeed = new List<NewsModel>();
            var importantNews = new List<NewsModel>();
            var newsFeedModel = new NewsfeedModel()
            {
                ImportantNews = importantNews,
                Newsfeed = newsfeed
            };

            mockedNewsfeedView.SetupGet(x => x.Model).Returns(newsFeedModel);

            mockedNewsfeedView.Raise(x => x.EventBindNewsfeedData += null, EventArgs.Empty);

            Assert.AreSame(newsFeedModel.Newsfeed, mockedNewsfeedView.Object.Model.Newsfeed);
            Assert.AreSame(newsFeedModel.ImportantNews, mockedNewsfeedView.Object.Model.ImportantNews);
        }
    }
}
