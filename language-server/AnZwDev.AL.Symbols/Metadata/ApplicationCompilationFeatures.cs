using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public struct ApplicationCompilationFeatures
    {

          public bool TranslationFile { get; set; }
          public bool GenerateCaptions { get; set; }
          public bool ExcludeGeneratedTranslations { get; set; }
          public bool NoImplicitWith { get; set; }
          public bool NoPromotedActionProperties { get; set; }
          public bool GenerateLockedTranslations { get; set; }
          public bool AllTranslationItems { get; set; }
          public bool UseLegacyAnalyzerStrategy { get; set; }

    }
}
