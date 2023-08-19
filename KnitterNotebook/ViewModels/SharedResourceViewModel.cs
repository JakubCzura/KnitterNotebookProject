using System;

namespace KnitterNotebook.ViewModels
{
    public class SharedResourceViewModel
    {
        public string? PatternPdfPath { get; set; }

        public Action PatternPdfPathChanged { get; set; } = null!;

        public void OnPatternPdfPathChanged()
        {
            PatternPdfPathChanged?.Invoke();
        }
    }
}