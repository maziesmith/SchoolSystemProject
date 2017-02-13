﻿using System.Collections.Generic;

using SchoolSystem.Data.Models;
using SchoolSystem.Data.Models.CustomModels;

namespace SchoolSystem.Web.Services.Contracts
{
    public interface ISubjectManagementService
    {
        bool CreateSubject(string subjectName, string subjectPictureUrl);

        IEnumerable<Subject> GetAllSubjects();

        IEnumerable<Subject> GetAllSubjectsAlreadyAssignedToTheClass(int classId);

        IEnumerable<SubjectBasicInfo> GetSubjectsPerTeacher(string teacherName, bool isAdmin);

        /// <summary>
        /// Gets the subjects with no assigned teacher
        /// </summary>
        /// <returns> IEnumerable<Subject> </returns>
        IEnumerable<SubjectBasicInfo> GetAllSubjectsWithoutTeacher();

        /// <summary>
        /// Gets the subjects that are not assigned to the specified class yet.
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        IEnumerable<SubjectBasicInfo> GetSubjectsNotYetAssignedToTheClass(int classId);
    }
}
