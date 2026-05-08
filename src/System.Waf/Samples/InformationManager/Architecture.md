# Architecture

## Namespace (Layer) diagram

```mermaid
flowchart TB

  subgraph ASM["Assembler"]
  end

  subgraph CP["Common.Presentation"]
    direction LR
    CP_CT["Controls"]
    CP_C["Converters"]
  end

  subgraph CA["Common.Applications"]
    direction LR
    CA_S["Services"]
  end

  subgraph II["Infrastructure.Interfaces"]
    direction LR
    II_A["Applications"]
  end

  subgraph IMA["Infrastructure.Modules.Applications"]
    direction LR
    IMA_P["Properties"]
    IMA_C["Controllers"]
    IMA_S["Services"]
    IMA_VM["ViewModels"]
    IMA_V["Views"]
  end

  subgraph IMP["Infrastructure.Modules.Presentation"]
    direction LR
    IMP_C["Converters"]
    IMP_S["Services"]
    IMP_V["Views"]
  end

  subgraph AI["AddressBook.Interfaces"]
    direction LR
    AI_A["Applications"]
    AI_D["Domain"]
  end

  subgraph AD["AddressBook.Modules.Domain"]
  end

  subgraph AA["AddressBook.Modules.Applications"]
    direction LR
    AA_C["Controllers"]
    AA_SD["SampleData"]
    AA_VM["ViewModels"]
    AA_V["Views"]
  end

  subgraph AP["AddressBook.Modules.Presentation"]
    direction LR
    AP_V["Views"]
  end

  subgraph ED["EmailClient.Modules.Domain"]
    direction LR
    ED_AS["AccountSettings"]
    ED_E["Emails"]
  end

  subgraph EA["EmailClient.Modules.Applications"]
    direction LR
    EA_C["Controllers"]
    EA_SD["SampleData"]
    EA_VM["ViewModels"]
    EA_V["Views"]
  end

  subgraph EP["EmailClient.Modules.Presentation"]
    direction LR
    EP_C["Converters"]
    EP_S["Selectors"]
    EP_V["Views"]
  end

  ASM --> II
  ASM --> CA
  ASM --> CP

  IMA --> II
  
  IMP --> IMA
  IMP --> CA

  AA --> AI
  AA --> AD
  AA --> II

  AP --> AA
  AP --> CP

  EA --> AI
  EA --> ED
  EA --> II

  EP --> EA
  EP --> CP

  AI_A --> AI_D

  ED_E --> ED_AS

  IMA_C --> IMA_VM
  IMA_VM --> IMA_S
  IMA_VM --> IMA_V

  IMP_V --> IMP_C

  AA_C --> AA_SD
  AA_C --> AA_VM
  AA_VM --> AA_V

  EA_C --> EA_SD
  EA_C --> EA_VM
  EA_VM --> EA_V

  EP_V --> EP_C
  EP_V --> EP_S
```

## Dependency Rules

[config.nsdepcop](./config.nsdepcop)
