﻿using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UITest.BookLibrary.Controls;

public class HyperlinkGridCell(FrameworkAutomationElementBase element) : GridCell(element)
{
    public AutomationElement RootElement => FindFirstChild();

    public AutomationElement Hyperlink => this.Find(x => x.ByControlType(ControlType.Hyperlink));

    public Label Label => Hyperlink.Find(x => x.ByControlType(ControlType.Text)).AsLabel();
}
