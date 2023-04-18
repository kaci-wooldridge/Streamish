using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }
        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.Name, up.Email, up.ImageUrl, up.DateCreated
                        FROM UserProfile up";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var users = new List<UserProfile>();
                        while (reader.Read())
                        {
                            users.Add(new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                            });
                        }

                        return users;
                    }
                }
            }
        }
        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.Name, up.Email, up.ImageUrl, up.DateCreated
                        FROM UserProfile up
                        WHERE up.Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        UserProfile user = null;
                        if (reader.Read())
                        {
                            user = new UserProfile()
                            {
                                Id = id,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                            };
                        }

                        return user;
                    }
                }
            }
        }
        public void Add(UserProfile user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (FirebaseUserId, Name, Email, DateCreated, ImageUrl)
                        OUTPUT INSERTED.ID
                        VALUES (@FirebaseUserId, @Name, @Email, @DateCreated, @ImageUrl)";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", user.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@Name", user.Name);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", user.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", user.ImageUrl);

                    user.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void Update(UserProfile user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                           SET Name = @Name,
                               Email = @Email,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Name", user.Name);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", user.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", user.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Id", user.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public UserProfile GetUserProfileByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.[Name], up.Email, up.DateCreated, up.ImageUrl,

                            v.Id AS VideoId, v.Title, v.Description, v.DateCreated AS VideoDateCreated, v.Url, v.UserProfileId AS VideoUserProfileId,

                            c.Id AS CommentId, c.VideoId AS CommentVideoId, c.UserProfileId AS CommentUserProfileId, c.Message
                        FROM UserProfile up
                            LEFT JOIN Video v ON v.UserProfileId = up.Id
                            LEFT JOIN Comment c ON c.VideoId = v.Id
                        WHERE up.Id = @Id
                        ORDER BY v.DateCreated DESC";
                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        UserProfile userProfile = null;

                        while (reader.Read())
                        {
                            if (userProfile == null)
                            {
                                userProfile = new UserProfile()
                                {
                                    Id = DbUtils.GetInt(reader, "Id"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                            }

                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                var videoId = DbUtils.GetInt(reader, "VideoId");

                                var existingVideo = userProfile.Videos.FirstOrDefault(p => p.Id == videoId);
                                if (existingVideo == null)
                                {
                                    existingVideo = new Video()
                                    {
                                        Id = videoId,
                                        Title = DbUtils.GetString(reader, "Title"),
                                        Description = DbUtils.GetString(reader, "Description"),
                                        DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                        Url = DbUtils.GetString(reader, "Url"),
                                        UserProfileId = DbUtils.GetInt(reader, "VideoUserProfileId"),
                                        Comments = new List<Comment>()
                                    };

                                    userProfile.Videos.Add(existingVideo);
                                }

                                if (DbUtils.IsNotDbNull(reader, "CommentId"))
                                {
                                    existingVideo.Comments.Add(new Comment()
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentId"),
                                        Message = DbUtils.GetString(reader, "Message"),
                                        VideoId = videoId,
                                        UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                                    });
                                }
                            }
                        }

                        return userProfile;
                    }
                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.FirebaseUserId, up.[Name], up.Email, up.DateCreated, up.ImageUrl,

                            v.Id AS VideoId, v.Title, v.Description, v.DateCreated AS VideoDateCreated, v.Url, v.UserProfileId AS VideoUserProfileId,

                            c.Id AS CommentId, c.VideoId AS CommentVideoId, c.UserProfileId AS CommentUserProfileId, c.Message
                        FROM UserProfile up
                            LEFT JOIN Video v ON v.UserProfileId = up.Id
                            LEFT JOIN Comment c ON c.VideoId = v.Id
                         WHERE FirebaseUserId = @FirebaseuserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        UserProfile userProfile = null;

                        while (reader.Read())
                        {
                            if (userProfile == null)
                            {
                                userProfile = new UserProfile()
                                {
                                    Id = DbUtils.GetInt(reader, "Id"),
                                    FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                            }

                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                var videoId = DbUtils.GetInt(reader, "VideoId");

                                var existingVideo = userProfile.Videos.FirstOrDefault(p => p.Id == videoId);
                                if (existingVideo == null)
                                {
                                    existingVideo = new Video()
                                    {
                                        Id = videoId,
                                        Title = DbUtils.GetString(reader, "Title"),
                                        Description = DbUtils.GetString(reader, "Description"),
                                        DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                        Url = DbUtils.GetString(reader, "Url"),
                                        UserProfileId = DbUtils.GetInt(reader, "VideoUserProfileId"),
                                        Comments = new List<Comment>()
                                    };

                                    userProfile.Videos.Add(existingVideo);
                                }

                                if (DbUtils.IsNotDbNull(reader, "CommentId"))
                                {
                                    existingVideo.Comments.Add(new Comment()
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentId"),
                                        Message = DbUtils.GetString(reader, "Message"),
                                        VideoId = videoId,
                                        UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                                    });
                                }
                            }
                        }

                        return userProfile;
                    }
                }
            }
        }

    }
}
