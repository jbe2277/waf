# Architecture

## Namespace (Layer) diagram

```mermaid
flowchart TB

  subgraph LP["Library.Presentation"]
    direction LR
    LP_P["Properties"]
    LP_C["Converters"]
    LP_D["Data"]
    LP_S["Services"]
    LP_V["Views"]
  end

  subgraph LA["Library.Applications"]
    direction LR
    LA_P["Properties"]
    LA_C["Controllers"]
    LA_D["Data"]
    LA_DM["DataModels"]
    LA_S["Services"]
    LA_VM["ViewModels"]
    LA_V["Views"]
  end

  subgraph D["Library.Domain"]
  end

  subgraph RP["Reporting.Presentation"]
    direction LR
    RP_CT["Controls"]
    RP_R["Reports"]
    RP_V["Views"]
  end

  subgraph RA["Reporting.Applications"]
    direction LR
    RA_C["Controllers"]
    RA_DM["DataModels"]
    RA_R["Reports"]
    RA_VM["ViewModels"]
    RA_V["Views"]
  end

  LP --> LA
  LA --> D
  RP --> RA
  RA --> LA

  LP_S --> LP_D
  LP_V --> LP_C
  LP_V --> LP_S

  LA_C --> LA_D
  LA_C --> LA_DM
  LA_C --> LA_S
  LA_C --> LA_VM
  LA_S --> LA_V
  LA_VM --> LA_DM
  LA_VM --> LA_S
  LA_VM --> LA_V

  RA_C --> RA_DM
  RA_C --> RA_R
  RA_C --> RA_VM
  RA_DM --> RA_R
  RA_VM --> RA_R
  RA_VM --> RA_V

  RP_R --> RP_CT
  RP_V --> RP_R
```

## Dependency Rules

[config.nsdepcop](./config.nsdepcop)
