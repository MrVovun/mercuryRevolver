using System.Collections.Generic;
using UnityEngine;

namespace MR.Systems.Cases
{
    public class CaseService
    {
        private readonly HashSet<string> foundClues = new();
        public CaseDef currentCase;

        public void StartCase(CaseDef caseDef)
        {
            currentCase = caseDef;
            foundClues.Clear();
        }
        public void RegisterClue(ClueDef clue) => foundClues.Add(clue.clueID);

        public bool CanRepose()
        {
            if (currentCase == null) return false;
            foreach (var clue in currentCase.clues)
            {
                if (!foundClues.Contains(clue.clueID))
                    return false;
            }
            return true;
        }
        public CaseDef ActiveCase() => currentCase;
    }
}