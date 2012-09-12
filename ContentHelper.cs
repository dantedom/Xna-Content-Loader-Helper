using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Text;
using System.Xml;
using System.Globalization;
 
namespace LoaderHelper
{
   
    public delegate object CustomLoader(string path);
    public class ContentHelper
    {
        public ContentManager contentmanager;
        public CustomLoader customLoader;
        public ContentHelper(ContentManager contentmanager = null, CustomLoader customLoader = null, string extention =null)
        {
            this.contentmanager = contentmanager;
            this.customLoader = customLoader;
            if (!string.IsNullOrEmpty(extention)) { SupportedExtentions.Add(extention); }
        }
 

        public List<string> SupportedExtentions = new List<string> { "xnb", "anim", "scen" };

        //TODO add items to input dictionary if not null
        public Dictionary<string, Texture2D> getDictionary( string path, Dictionary<string, Texture2D> input)
        {

            return (Dictionary<string, Texture2D>)getDictionary(path, ContentType.Texture2D, input);

        }
        public Dictionary<string, Texture> getDictionary(  string path, Dictionary<string, Texture> input)
        {

            return (Dictionary<string, Texture>)getDictionary(path, ContentType.Texture, input);

        }
        public Dictionary<string, Song> getDictionary( string path, Dictionary<string, Song> input)
        {

            return (Dictionary<string, Song>)getDictionary(path, ContentType.Song, input);

        }
        public Dictionary<string, Video> getDictionary(  string path, Dictionary<string, Video> input)
        {

            return (Dictionary<string, Video>)getDictionary(path, ContentType.Video, input);

        }
        public Dictionary<string, Effect> getDictionary(  string path, Dictionary<string, Effect> input)
        {

            return (Dictionary<string, Effect>)getDictionary(path, ContentType.Effect, input);

        }
        public Dictionary<string, SoundEffect> getDictionary( string path, Dictionary<string, SoundEffect> input)
        {

            return (Dictionary<string, SoundEffect>)getDictionary(path, ContentType.SoundEffect, input);

        }
        public Dictionary<string, SpriteEffect> getDictionary(  string path, Dictionary<string, SpriteEffect> input)
        {

            return (Dictionary<string, SpriteEffect>)getDictionary(path, ContentType.SpriteEffect, input);

        }
        private object getDictionary(ContentManager Content, string path, ContentType type,object input  = null )
        {
            Dictionary<string, object> container = ContentLoader(Content, path, type);
            if (container == null)
            {
                return null;
            }


            object result = null;
            switch (type)
            {
                case ContentType.Texture2D:
                    var Texture2D_values = input == null? new Dictionary<string, Texture2D>(): (Dictionary<string, Texture2D>)input;
                    foreach (var item in container.Keys)
                    {
                        Texture2D_values[item] = (Texture2D)container[item];
                    }
                    result = Texture2D_values;
                    break;

                case ContentType.Texture:
                    var Texture_values = input == null ? new Dictionary<string, Texture>() : (Dictionary<string, Texture>)input;

                    foreach (var item in container.Keys)
                    {
                        Texture_values[item] = (Texture)container[item];
                    }
                    result = Texture_values;
                    break;

                case ContentType.Song:
                    var Song_values = input == null ? new Dictionary<string, Song>() : (Dictionary<string, Song>)input;
                    foreach (var item in container.Keys)
                    {
                        Song_values[item] = (Microsoft.Xna.Framework.Media.Song)container[item];
                    }
                    result = Song_values;
                    break;

                case ContentType.Video:
                    var Video_values = input == null ? new Dictionary<string, Video>() : (Dictionary<string, Video>)input;
                    foreach (var item in container.Keys)
                    {
                        Video_values[item] = (Microsoft.Xna.Framework.Media.Video)container[item];
                    }
                    result = Video_values;
                    break;

                case ContentType.SoundEffect:
                    var SoundEffect_values = input == null ? new Dictionary<string, SoundEffect>() : (Dictionary<string, SoundEffect>)input;
                    foreach (var item in container.Keys)
                    {
                        SoundEffect_values[item] = (Microsoft.Xna.Framework.Audio.SoundEffect)container[item];
                    }
                    result = SoundEffect_values;
                    break;

                case ContentType.SpriteEffect:
                    var SpriteEffect_values = input == null ? new Dictionary<string, SpriteEffect>() : (Dictionary<string, SpriteEffect>)input;
                    foreach (var item in container.Keys)
                    {
                        SpriteEffect_values[item] = (SpriteEffect)container[item];
                    }
                    result = SpriteEffect_values;
                    break;

                case ContentType.Effect:
                    var Effect_values = input == null ? new Dictionary<string, Effect>() : (Dictionary<string, Effect>)input;
                    foreach (var item in container.Keys)
                    {
                        Effect_values[item] = (Effect)container[item];
                    }
                    result = Effect_values;

                    break;

                default:
                    break;
            }

            return result;
        }
        private object getDictionary(string path, ContentType type, object input =null) { return getDictionary(this.contentmanager, path, type,input); }
        public Dictionary<string, object> ContentLoader(ContentManager contentManager, string folderPath, ContentType T)
        {
            char ds = System.IO.Path.DirectorySeparatorChar;
            string[] filenames = new string[0];

            Dictionary<string, object> result = new Dictionary<string, object>();
            bool success = false;
            string path = folderPath + ds;
            string contentF = contentManager != null ? contentManager.RootDirectory + ds + folderPath : folderPath;
#if ANDROID 
   
            var nlist =Game.Activity.Assets.List(contentF );
			filenames = nlist; 
			if(filenames.Length ==0) { throw new  Exception("no file names"); }
//path = contentF + ds;
#else

            DirectoryInfo dir = new DirectoryInfo(contentF);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException();
            }
            FileInfo[] fs = dir.GetFiles();
            filenames = new string[fs.Length];


#endif

            object content = null;

            for (int i = 0; i < filenames.Length; i++)
            {
                string key = "";
#if ANDROID
                     key = filenames[i];

#else
                key = fs[i].Name;
#endif

                string[] K = key.Split('.');
                if (K.Length == 1)
                {
                    continue;
                }


                string ext = K[1];
                if (!SupportedExtentions.Contains(ext))
                {
                    continue;
                }

                key = K[0];

                switch (T)
                {
                    case ContentType.Texture2D:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<Texture2D>(path + key));
                        }
                        break;
                    case ContentType.Texture:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<Texture>(path + key));
                        }
                        break;
                    case ContentType.Video:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<Microsoft.Xna.Framework.Media.Video>(path + key));
                        }
                        break;

                    case ContentType.Song:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<Microsoft.Xna.Framework.Media.Song>(path + key));
                        }
                        break;
                    case ContentType.Sound:
                        break;
                    case ContentType.SoundEffect:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(path + key));
                        }
                        break;
                    case ContentType.SpriteEffect:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<SpriteFont>(path + key));
                        }
                        break;
                    case ContentType.Effect:
                        if (contentManager != null)
                        {
                            content = (contentManager.Load<Effect>(path + key));
                        }
                        break;

                    case ContentType.Custom:
                        if (customLoader != null)
                        {
                            content = customLoader(path + key + "." + ext);

                        }
                        break;
                    default:


                        break;
                }

                if (content != null)
                {
                    result[key] = content;
                    success = true;
                }
            }
            if (success)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, object> ContentLoader( string folderPath, ContentType T)
        {
           return ContentLoader(this.contentmanager, folderPath, T);
        }
    }
}
