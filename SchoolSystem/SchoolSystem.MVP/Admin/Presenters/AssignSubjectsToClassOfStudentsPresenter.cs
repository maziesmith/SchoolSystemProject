﻿using System;
using Bytes2you.Validation;
using SchoolSystem.MVP.Admin.Views;
using SchoolSystem.MVP.Admin.Views.EventArguments;
using SchoolSystem.Web.Services.Contracts;
using WebFormsMvp;

namespace SchoolSystem.MVP.Admin.Presenters
{
    public class AssignSubjectsToClassOfStudentsPresenter : Presenter<IAssignSubjectsToClassOfStudentsView>
    {
        private readonly IClassOfStudentsManagementService classOfStudentManagementService;
        private readonly ISubjectManagementService subjectManagementService;

        public AssignSubjectsToClassOfStudentsPresenter(
            IAssignSubjectsToClassOfStudentsView view,
            IClassOfStudentsManagementService classOfStudentManagementService,
            ISubjectManagementService subjectManagementService)
            : base(view)
        {
            Guard.WhenArgument(classOfStudentManagementService, "classOfStudentManagementService").IsNull().Throw();
            Guard.WhenArgument(subjectManagementService, "subjectManagementService").IsNull().Throw();

            this.classOfStudentManagementService = classOfStudentManagementService;
            this.subjectManagementService = subjectManagementService;

            this.View.EventGetAllClassOfStudents += this.View_EventGetAllClassOfStudents;
            this.View.EventGetAvailableSubjectsForTheClass += this.View_EventGetAvailableSubjectsForTheClass;
            this.View.EventAssignSubjectsToClassOfStudents += this.View_EventAssignSubjectsToClassOfStudents;
        }

        private void View_EventAssignSubjectsToClassOfStudents(object sender, AssignSubjectsToClassOfStudentsEventArgs e)
        {
            this.View.Model.IsAddingSubjectsSuccesfull =
                this.classOfStudentManagementService.AddSubjectsToClass(e.ClassOfStudentsId, e.SubjectIdsToBeAdded);
        }

        private void View_EventGetAvailableSubjectsForTheClass(object sender, GetAvailableSubjectsForTheClassEventArgs e)
        {
            this.View.Model.AvailableSubjects = 
                this.subjectManagementService.GetSubjectsNotYetAssignedToTheClass(e.ClassOfStudentsId);
        }

        private void View_EventGetAllClassOfStudents(object sender, EventArgs e)
        {
            this.View.Model.ClassOfStudents = this.classOfStudentManagementService.GetAllClasses();
        }
    }
}