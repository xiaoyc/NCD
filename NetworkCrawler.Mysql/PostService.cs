using System;
using Dapper;
using NetworkCrawler.Mysql.Models;
using Dapper.Contrib.Extensions;

namespace NetworkCrawler.Mysql
{
    
    public class PostService :Service
    {
        CategoryService categoryService = new CategoryService();
        TagService tagService = new TagService();
        ActorService actorService = new ActorService();

        public  void GetPosts()
        {
            var posts =  conn.GetAll<Post>();
        }

        public Post GetPostByTitle(string postTitle)
        {
            var posts = conn.Query<Post>("select * from post where title =@Title", new { Title = postTitle }).AsList();

            if (posts.Count > 0)
            {
                return posts[0];
            }

            return null;
        }

        public long InsertPost(Post p , bool forceInsert)
        {
            long postId = 0;            

            
            var categoryId = categoryService.InsertCategory(p.CategoryName);

            if (!forceInsert)
            {
                var post = GetPostByTitle(p.Title);

                if (post != null)
                {
                    postId = post.Id;
                }
               
            }
            else
            {
                conn.Execute("delete from post where postId =@PostId", new { PostId = postId });
            }

            if (postId == 0)
                postId = conn.Insert(p);

            if (p.Tags != null && p.Tags.Length > 0)
            {
                InsertPostTag(postId, p.Tags);
            }

            if (p.Actors != null && p.Actors.Length > 0)
            {
                InsertPostActor(postId, p.Actors);
            }

            return postId;
        }

        public long InsertPostTag(long postId, string[] tags)
        {
            int count = 0;
            conn.Execute("delete PostTag where postId =@PostId ", new { PostId = postId });

            foreach (var tag in tags)
            {
                var tagId = tagService.InsertTag(tag);

                conn.Insert(new PostTagMapping() { PostId = postId, TagId = tagId });
                count++;
            }

            return count;
        }


        public long InsertPostActor(long postId, string[] actors)
        {
            int count = 0;
            conn.Execute("delete PostActor where postId =@PostId ", new { PostId = postId });

            foreach (var actor in actors)
            {
                var actorId = actorService.InsertActor(actor);

                conn.Insert(new PostActorMapping() { PostId = postId, ActorId = actorId});
                count++;
            }

            return count;
        }
    }
}
