﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SchoolSystem.Data.Contracts;
using SchoolSystem.Data.Models;
using SchoolSystem.Web.Services;

namespace SchoolSystem.Services.Tests.ClassOfStudentsManagemtServiceTests
{
    [TestFixture]
    public class AddClass_Should
    {
        private const string NotNullString = "notNull";

        [Test]
        public void ThrowArgumentNullException_WhenNameParamIsNuull()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();
            var mockedUnitOfWork = new Mock<Func<IUnitOfWork>>();

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, mockedUnitOfWork.Object);
            IEnumerable<string> subjectIds = new List<string>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                service.AddClass(null, subjectIds);
            });
        }

        [Test]
        public void ThrowArgumentException_WhenNameParamIsEmpty()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<Func<IUnitOfWork>>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, mockedUnitOfWork.Object);

            IEnumerable<string> subjectIds = new List<string>();

            Assert.Throws<ArgumentException>(() =>
            {
                service.AddClass(string.Empty, subjectIds);
            });
        }

        [Test]
        public void ThrowArgumentException_WithMessageContatining_Name_WhenNameParamIsEmpty()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<Func<IUnitOfWork>>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, mockedUnitOfWork.Object);
            IEnumerable<string> subjectIds = new List<string>();

            Assert.That(
                () =>
                service.AddClass(string.Empty, subjectIds),
            Throws.ArgumentException.With.Message.Contain("name"));
        }

        [Test]
        public void ThrowArgumentNullException_WithMessageContatining_Name_WhenNameParamIsNuull()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<Func<IUnitOfWork>>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, mockedUnitOfWork.Object);
            IEnumerable<string> subjectIds = new List<string>();

            Assert.That(
                () => service.AddClass(null, subjectIds),
             Throws.ArgumentNullException.With.Message.Contain("name"));
        }

        [Test]
        public void ThrowArgumentNullException_WhenSubjectIdsParamIsNuull()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<Func<IUnitOfWork>>();

            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, mockedUnitOfWork.Object);

            Assert.Throws<ArgumentNullException>(
                () => service.AddClass(NotNullString, null));
        }

        [Test]
        public void ThrowArgumentNullException_WithMessageContaining_Subjects_WhenSubjectIdsParamIsNuull()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<Func<IUnitOfWork>>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, mockedUnitOfWork.Object);

            Assert.That(
                () => service.AddClass(NotNullString, null),
             Throws.ArgumentNullException.With.Message.Contain("subjecIds"));
        }

        [Test]
        public void Return_False_WhenThereIsClassOfStudentsWithTheSameName()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            string className = NotNullString;
            IEnumerable<string> classNames = new List<string>() { className };

            mockedClassOfStudentsRepo.Setup(x => x.GetAll(null, y => y.Name)).Returns(classNames);

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, () => mockedUnitOfWork.Object);
            var nonEmptyList = new List<string>() { "2" };

            var result = service.AddClass(NotNullString, nonEmptyList);

            Assert.False(result);
        }

        [Test]
        public void Call_AddMethod_Once_FromClassOfStudentsRepoOnce()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            IEnumerable<string> classNames = new List<string>() { };

            mockedClassOfStudentsRepo.Setup(x => x.GetAll(null, y => y.Name)).Returns(classNames);
            mockedClassOfStudentsRepo.Setup(x => x.Add(It.IsAny<ClassOfStudents>()));
            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, () => mockedUnitOfWork.Object);
            var nonEmptyList = new List<string>() { "2" };

            service.AddClass(NotNullString, nonEmptyList);

            mockedClassOfStudentsRepo.Verify(x => x.Add(It.IsAny<ClassOfStudents>()), Times.Once);
        }

        [Test]
        public void Call_CommitMethod_Once()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            mockedUnitOfWork.Setup(x => x.Commit());

            IEnumerable<string> classNames = new List<string>() { };

            mockedClassOfStudentsRepo.Setup(x => x.GetAll(null, y => y.Name)).Returns(classNames);
            mockedClassOfStudentsRepo.Setup(x => x.Add(It.IsAny<ClassOfStudents>()));
            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, () => mockedUnitOfWork.Object);
            var nonEmptyList = new List<string>() { "2" };

            service.AddClass(NotNullString, nonEmptyList);

            mockedUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }

        [Test]
        public void Return_True_WhenCreatingClassIsSuccesfull()
        {
            var mockedClassOfStudentsRepo = new Mock<IRepository<ClassOfStudents>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedSubjectClassOfStudentsRepo = new Mock<IRepository<SubjectClassOfStudents>>();

            mockedUnitOfWork.Setup(x => x.Commit()).Returns(true);

            IEnumerable<string> classNames = new List<string>();

            mockedClassOfStudentsRepo.Setup(x => x.GetAll(null, y => y.Name)).Returns(classNames);

            var service = new ClassOfStudentsManagementService(mockedClassOfStudentsRepo.Object, mockedSubjectClassOfStudentsRepo.Object, () => mockedUnitOfWork.Object);
            var nonEmptyList = new List<string>() { "2" };

            var result = service.AddClass(NotNullString, nonEmptyList);

            Assert.True(result);
        }
    }
}
