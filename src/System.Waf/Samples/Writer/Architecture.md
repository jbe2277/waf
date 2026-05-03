# Architecture

## Namespace (Layer) diagram

```mermaid
flowchart TB

  subgraph P["Presentation"]
    direction LR
    P_P["Properties"]
    P_C["Converters"]
    P_DD["DesignData"]
    P_S["Services"]
    P_V["Views"]
  end

  subgraph A["Applications"]
    direction LR
    A_P["Properties"]
    A_C["Controllers"]
    A_D["Documents"]
    A_S["Services"]
    A_VM["ViewModels"]
    A_V["Views"]
  end

  P --> A

  P_C --> P_S
  P_DD --> P_S
  P_DD --> P_V
  P_V --> P_C
  P_V --> P_S

  A_C --> A_D
  A_C --> A_S
  A_C --> A_VM
  A_D --> A_S
  A_S --> A_V
  A_VM --> A_D
  A_VM --> A_S
  A_VM --> A_V
```

## Dependency Rules

[config.nsdepcop](./config.nsdepcop)
