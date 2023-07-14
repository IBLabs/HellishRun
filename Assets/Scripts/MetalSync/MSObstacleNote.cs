using System;

namespace MetalSync
{
    [Serializable]
    public class MSObstacleNote
    {
        public string identifier;
        public int beatCount;

        public MSObstacleNote(string identifier, int beatCount)
        {
            this.identifier = identifier;
            this.beatCount = beatCount;
        }
    }
}