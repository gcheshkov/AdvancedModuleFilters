﻿using System.Collections.Generic;
using System.ComponentModel;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Helpers;

namespace TSLab.AdvancedModuleFilters
{
  [HandlerCategory("Сборник фильтров")]
  [HandlerName("Filter CCI")]
  [Input(0, TemplateTypes.SECURITY, Name = "Источник")]
  [Description("Значение индикатора CCI больше или меньше заданного значения.")]
  public class AdvFilterCCI : IBar2BoolsHandler, IOneSourceHandler, IBooleanReturns, IStreamHandler, IHandler, ISecurityInputs, IContextUses
  {
    [HandlerParameter(Default = "true", IsShown = false, Name = "CCI > k", NotOptimized = true)]
    public bool ParBol { get; set; }

    [HandlerParameter(Default = "20", IsShown = false, Max = "205", Min = "5", Name = "Период", NotOptimized = false, Step = "100")]
    public int ParPeriod { get; set; }

    [HandlerParameter(Default = "5", IsShown = false, Max = "100", Min = "0", Name = "Значение k", NotOptimized = false, Step = "5")]
    public double ParValue { get; set; }

    public IContext Context { get; set; }

    public IList<bool> Execute(ISecurity source)
    {
      IReadOnlyList<IDataBar> bars = source.Bars;
      int count = bars.Count;
      IList<double> doubleList = Series.CCI(bars, this.ParPeriod, (IMemoryContext) this.Context);
      IList<bool> boolList = (IList<bool>) (this.Context?.GetArray<bool>(count) ?? new bool[count]);
      for (int index = 0; index < count; ++index)
        boolList[index] = this.ParBol ? doubleList[index] > this.ParValue : doubleList[index] < this.ParValue;
      return boolList;
    }
  }
}
