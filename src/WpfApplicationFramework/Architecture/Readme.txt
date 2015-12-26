Readme: Architecture
--------------------


1. Build Configuration

a) Set Validate Architecture = True of the Architecture project (in Properties Window).

b) Configuration Manager: Deactivate the build of the Architecture project in Debug mode (improve build time).

   Attention: The Validate Architecture feature works only in Release mode anymore.


2. Layer diagrams
   
Configure the "Forbidden Namespace Dependencies" (in Properties Window).

Applications layer: 
   System.Windows.Controls;System.Windows.Data;System.Windows.Media;System.Media;System.Waf.Presentation

Domain layer (and all layers below Domain):
   System.Windows;System.Media;System.Waf.Presentation;System.Waf.Applications
