using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKE2018_Challenge___Task_2
{
    struct wordAfter
    {
        public string word;
        public string tag;
        public string placeInSentence;
    }

    struct wordBefore
    {
        public string word;
        public string tag;
        public string placeInSentence;
    }


    class Entity
    {
        public string word;
        public string tag;
        public int begin;
        public int end;
        public string placeInSentence;
        public wordBefore wordBefore;
        public wordAfter wordAfter;
    }

    class JSON_entity
    {
        public string name;
        public int begin_index;
        public int end_index;
        public string label;
        public string link;
    }
}
