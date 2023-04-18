using Streamish.Models;
using System.Collections.Generic;

namespace Streamish.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile user);
        void Delete(int id);
        List<UserProfile> GetAll();
        UserProfile GetById(int id);
        void Update(UserProfile user);
        UserProfile GetUserProfileByIdWithVideos(int id);
        public UserProfile GetByFirebaseUserId(string firebaseUserId);
    }
}