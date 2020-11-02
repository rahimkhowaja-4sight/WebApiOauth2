using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOauth2.Areas.HelpPage.ModelDescriptions;

namespace WebApiOauth2.Models
{
    public class RequestModels
    {
        [ModelName("RequestModelVerifyOTP")]
        public class RequestModelVerifyOTP
        {
            public string OTP;
            public string UserName;
            public string deviceUDID;
        }

        [ModelName("RequestModelRegisterUsers")]
        public class RequestModelRegisterUsers
        {
            public string userName { get; set; }
            public string passWord { get; set; }
            public string fullName { get; set; }
            //public string emirateID { get; set; }
            //public string licenseNo { get; set; }
            public string emailAddr { get; set; }
            public string addressHome { get; set; }
            public string pictureUrl { get; set; }
            public string phoneNo { get; set; }
        }

        [ModelName("RequestModelUpdateUserProfile")]
        public class RequestModelUpdateUserProfile
        {
            public string userName { get; set; }
            public string oldpassWord { get; set; }
            public string newpassWord { get; set; }
            public string fullName { get; set; }
            //public string emirateID { get; set; }
            //public string licenseNo { get; set; }
            public string emailAddr { get; set; }
            public string addressHome { get; set; }
            //public string pictureUrl { get; set; }
            public string phoneNo { get; set; }

            public string deviceDetails { get; set; }
            public string deviceUDID { get; set; }
            public string deviceTYPE { get; set; }
            public string mobileDatetime { get; set; }
            public string serviceTYPE { get; set; }

            public string base64FileObject { get; set; }
        }

        [ModelName("RequestModelGetUserDetails")]
        public class RequestModelGetUserDetails
        {
            public string userID { get; set; }
            public string userName { get; set; }
        }

        [ModelName("ListLocalizationByScreenName")]
        public class ListLocalizationByScreenName
        {
            public string ScreenName { get; set; }
        }

        [ModelName("RequestModelAddUpdateTask")]
        public class RequestModelAddUpdateTask
        {
            public string TaskID { get; set; }
            public string ParentTaskID { get; set; }
            public string Title_Eng { get; set; }
            public string Title_Arb { get; set; }
            public string TaskDescription_Eng { get; set; }
            public string TaskDescription_Arb { get; set; }
            public string Progress { get; set; }
            public string TaskStatusId { get; set; }
            public string TaskPriorityId { get; set; }
            public string TaskTypeId { get; set; }
            public string SessionUserID { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public List<string> NotifyUsers { get; set; }
            public List<AssignedUsersOrTeams> AssignedUsersOrTeams { get; set; }
            public List<string> base64FileObjects { get; set; }

        }

        public class AssignedUsersOrTeams
        {
            public string ID { get; set; }
            public string Type { get; set; }
        }

        [ModelName("RequestModelGetAllTasksList")]
        public class RequestModelGetAllTasksList
        {
            public string UserID { get; set; }
            public string TeamID { get; set; }
        }

        [ModelName("RequestModelGetAllParticipantsListForTask")]
        public class RequestModelGetAllParticipantsListForTask
        {
            public string TaskID { get; set; }
        }

        [ModelName("RequestModelGetTaskDetails")]
        public class RequestModelGetTaskDetails
        {
            public string TaskID { get; set; }
        }

        [ModelName("RequestModelGetAllUsersAndTeamsByUserID")]
        public class RequestModelGetAllUsersAndTeamsByUserID
        {
            public string UserID { get; set; }
        }

        [ModelName("RequestModelAddTaskComment")]
        public class RequestModelAddTaskComment
        {
            public string TaskID { get; set; }
            public string UserId { get; set; }
            public string Comment { get; set; }
            public List<string> base64FileObjects { get; set; }

        }

        [ModelName("RequestModelGetAllMeetingsList")]
        public class RequestModelGetAllMeetingsList
        {
            public string UserID { get; set; }
            public string TaskID { get; set; }
            public string EventID { get; set; }
        }


        [ModelName("RequestModelAddUpdateMeeting")]
        public class RequestModelAddUpdateMeeting
        {
            public string MeetingID { get; set; }
            public string TaskID { get; set; }
            public string EventID { get; set; }
            public string MeetingTitleEng { get; set; }
            public string MeetingTitleArb { get; set; }
            public string MeetingAgendaEng { get; set; }
            public string MeetingAgendaArb { get; set; }
            public string MeetingTypeEng { get; set; }
            public string MeetingTypeArb { get; set; }
            public string VirtualLink { get; set; }
            public string MeetingLocation { get; set; }
            public string MeetingLocationCoordinates { get; set; }
            public string MeetingDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string AssignPersonMOMID { get; set; }
            public List<AssignedUsersOrTeams> AssignedUsersOrTeams { get; set; }
            public string SessionUserID { get; set; }
            public string IsActive { get; set; }
            public string MOMItems { get; set; }
            public string MOMDate { get; set; }
            public List<string> base64FileObjects { get; set; }
        }

        [ModelName("RequestModelGetAllParticipantsListForMeeting")]
        public class RequestModelGetAllParticipantsListForMeeting
        {
            public string MeetingID { get; set; }
        }

        [ModelName("RequestModelGetMeetingDetails")]
        public class RequestModelGetMeetingDetails
        {
            public string MeetingID { get; set; }
        }

        [ModelName("RequestModelUpdateMeetingStatus")]
        public class RequestModelUpdateMeetingStatus
        {
            public string SessionUserID { get; set; }
            public string MeetingID { get; set; }
            public string IsActive { get; set; }
        }



        [ModelName("RequestModelGetEventsList")]
        public class RequestModelGetEventsList
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string EventType { get; set; }
            public string EventCode { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string RegStatus { get; set; }
        }


        [ModelName("RequestModelGetEventFeatures")]
        public class RequestModelGetEventFeatures
        {
            public string UserID { get; set; }
            public string EventId { get; set; }
            public string FeatureId { get; set; }
            public string SessionId { get; set; }
            public string EventCode { get; set; }
            public string SubSessionId { get; set; }
            public List<string> UserIDs { get; set; }
          
        }

        [ModelName("RequestModelGetSurveyDetailsByEventID")]
        public class RequestModelGetSurveyDetailsByEventID
        {
            public string UserID { get; set; }
            public string EventId { get; set; }
        }

        [ModelName("RequestModelPostSurveyDetails")]
        public class RequestModelPostSurveyDetails
        {
            public string SessionUserID { get; set; }
            public string EventID { get; set; }
            public string SurveyID { get; set; }
            public string SurveyDateTime { get; set; }
            public List<SurveyResponse> SurveyResponse { get; set; }
        }

        public class SurveyResponse
        {
            public string QuestionID { get; set; }
            public string AnswerID { get; set; }
            public string Other { get; set; }
        }

        [ModelName("RequestModelGetVotingDetailsByEventID")]
        public class RequestModelGetVotingDetailsByEventID
        {
            public string UserID { get; set; }
            public string EventId { get; set; }
        }

        [ModelName("RequestModelPostVotingDetails")]
        public class RequestModelPostVotingDetails
        {
            public string SessionUserID { get; set; }
            public string EventID { get; set; }
            public string VotingID { get; set; }
            public string VotingDateTime { get; set; }
            public List<VotingResponse> VotingResponse { get; set; }
        }

        public class VotingResponse
        {
            public string QuestionID { get; set; }
            public string AnswerID { get; set; }
            public string Other { get; set; }
        }

        [ModelName("RequestModelGetAllWallPosts")]
        public class RequestModelGetAllWallPosts
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string PostID { get; set; }
            public string PageNumber { get; set; }
        }

        [ModelName("RequestModelWallAddPost")]
        public class RequestModelWallAddPost
        {
            public string EventID { get; set; }
            public string FeatureID { get; set; }
            public string UserID { get; set; }
            public string Post_Content_Eng { get; set; }
            public string Post_Content_Arb { get; set; }
            public List<string> base64FileObjects { get; set; }
        }

        [ModelName("RequestModelPostLikeDislike")]
        public class RequestModelPostLikeDislike
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string PostID { get; set; }
            public string PostAttachmentID { get; set; }
        }

        [ModelName("RequestModelWallPostComment")]
        public class RequestModelWallPostComment
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string PostID { get; set; }
            public string PostAttachmentID { get; set; }
            public string PostCommentEng { get; set; }
            public string PostCommentArb { get; set; }
        }

        [ModelName("RequestModelGetPostComment")]
        public class RequestModelGetPostComment
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string PostID { get; set; }
            public string PostAttachmentID { get; set; }
            public string PageNumber { get; set; }
        }

        [ModelName("RequestModelGetPostLikes")]
        public class RequestModelGetPostLikes
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string PostID { get; set; }
            public string PostAttachmentID { get; set; }
            public string PageNumber { get; set; }
        }

        [ModelName("RequestModelGetAttendeesByEventID")]
        public class RequestModelGetAttendeesByEventID
        {
            public string UserID { get; set; }
            public string EventId { get; set; }
        }

        [ModelName("RequestUpdateAttendeeDetails")]
        public class RequestUpdateAttendeeDetails
        {
            public string UserID { get; set; }
            public string EventID { get; set; }
            public string AttendeeID { get; set; }
            public string Name { get; set; }
            public string Organization { get; set; }
            public string JobDescription { get; set; }
            public string PhoneNumber { get; set; }
        }

        [ModelName("RequestModelGetTriviaDetailsByEventID")]
        public class RequestModelGetTriviaDetailsByEventID
        {
            public string UserID { get; set; }
            public string EventId { get; set; }
        }

        [ModelName("RequestModelJoinOrAssignTrivia")]
        public class RequestModelJoinOrAssignTrivia
        {
            public string UserID { get; set; }
            public string AssignedtoUserID { get; set; }
            public string EventId { get; set; }
            public string TriviaId { get; set; }
        }

        [ModelName("RequestModelPostTriviaDetails")]
        public class RequestModelPostTriviaDetails
        {
            public string SessionUserID { get; set; }
            public string EventID { get; set; }
            public string TriviaID { get; set; }
            public string TriviaRelationID { get; set; }
            public List<TriviaResponse> TriviaResponse { get; set; }
        }

        public class TriviaResponse
        {
            public string QuestionID { get; set; }
            public string AnswerID { get; set; }
            public string Other { get; set; }
        }

        [ModelName("RequestModelGetChallengeDetailsByEventID")]
        public class RequestModelGetChallengeDetailsByEventID
        {
            public string UserID { get; set; }
            public string EventId { get; set; }
        }

        [ModelName("RequestModelJoinOrAssignChallenge")]
        public class RequestModelJoinOrAssignChallenge
        {
            public string UserID { get; set; }
            public string AssignedtoUserID { get; set; }
            public string EventId { get; set; }
            public string ChallengeId { get; set; }
        }

        [ModelName("RequestModelPostChallengeDetails")]
        public class RequestModelPostChallengeDetails
        {
            public string SessionUserID { get; set; }
            public string EventID { get; set; }
            public string ChallengeID { get; set; }
            public string ChallengeRelationID { get; set; }
            public List<ChallengeResponse> ChallengeResponse { get; set; }
        }

        public class ChallengeResponse
        {
            public string StepID { get; set; }
            public string ScannedBarCodeText { get; set; }
        }

        [ModelName("RequestModelGetTeamDetails")]
        public class RequestModelGetTeamDetails
        {
            public string TeamID { get; set; }
        }

      
        public class NotificationResponse
        {
            public string id { get; set; }
            public string action { get; set; }
            public string module { get; set; }
        }
    }
}