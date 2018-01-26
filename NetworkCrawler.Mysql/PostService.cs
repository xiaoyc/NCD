using System;
using Dapper;
using NetworkCrawler.Mysql.Models;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace NetworkCrawler.Mysql
{

    public class PostService : Service
    {
        CategoryService categoryService = new CategoryService();
        TagService tagService = new TagService();
        ActorService actorService = new ActorService();

        public void GetPosts()
        {
            var posts = conn.GetAll<Post>();
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

        public long InsertOrUpdatePost(Post p)
        {
            long postId = 0;


            var categoryId = categoryService.InsertCategory(p.CategoryName);


            var post = GetPostByTitle(p.Title);

            if (post != null)
            {
                conn.Update(p);
                postId = p.Id;
            }
            else
            {
                postId = conn.Insert(p);
            }

            if (!string.IsNullOrWhiteSpace(p.Tags))
            {
                InsertPostTag(postId, p.Tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }

            if (!string.IsNullOrWhiteSpace(p.Actors))
            {
                InsertPostActor(postId, p.Actors.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }

            return postId;
        }

        public IList<Post> GetDraftPosts()
        {
            return conn.Query<Post>(@"select * from post where isdraft=1").AsList();
        }
            public IList<Post> GetAllReadyPosts()
        {
            return conn.Query<Post>(@"select p1.*  ,GROUP_CONCAT(actor.name SEPARATOR ', ')  as actors  from (SELECT post.* ,GROUP_CONCAT(tag.Title SEPARATOR ', ')  as tags ,
                                     category.title as categoryName
                                     FROM post 
                                    join posttag on post.id = posttag.postid
                                    join tag on tag.id = posttag.TagId
                                    join category on category.id = post.categoryId
                                    where not exists ( select 1 from jPost where title = post.title )
                                    ) as p1
                                    join postactor on p1.id = postactor.postid
                                    join actor on actor.id = postactor.actorId
                                    GROUP BY p1.id
                                    ").AsList();
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

                conn.Insert(new PostActorMapping() { PostId = postId, ActorId = actorId });
                count++;
            }

            return count;
        }
    }
}
