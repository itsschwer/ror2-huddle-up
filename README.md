# loot objectives

A \[ client-side \] mod that adds a tooltip to the *Objectives* panel to track how much loot is left on a stage.

![tooltip sample screenshot](./xtra/demo.png?raw=true)

## what is tracked?

- multishop terminals *(counts individual terminals; includes equipment and shipping requests)*
- chests *(regular, large, legendary, category, category large)*
- adaptive chests *__<!> known issue__: counter doesn't decrement until rescan after an item has been selected*
- shrine chances *(counts all potential drops)*
- equipment barrels
- lockboxes *(includes void variant)*
- void cradles/potentials
- once teleporter is charged
    - cloaked chests
    - scrapper

<!--

## todo (maybe)
- config
    - what to display in tooltip
        - add lunar pods?
        - add blood shrines?
        - add mountain shrines?
        - add barrels?
    - only display after tp charged 
        - granular?

## wontdo
- add proper objectives — too convenient
    - *feel free to make a dependent mod that adds this feature though*

-->
