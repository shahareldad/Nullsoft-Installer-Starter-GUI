using InstallerGUI.Models;
using System.Collections.Generic;

namespace InstallerGUI.Contracts
{
    public interface IHoldVariables
    {
        IEnumerable<VariableModel> AllVariables { get; }
    }
}
