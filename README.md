# UnitMigration

Migrate all unit settings from a source RVT to a folder full of target RVT models.

## Change Project Units between Imperial and Metric With One Button

This add-in has been created and published in response to the Revit Idea Station request by Kasper Miller
to [change project units between imperial and metric with one button](https://forums.autodesk.com/t5/revit-ideas/change-project-units-between-imperial-and-metric-with-one-button/idi-p/9235848).

Kasper explains:

This should be easy implement and will save a lot of time.

Currently, the process of converting an Imperial Revit family, project or template to metric or vice versa is very cumbersome.

Particularly if you have do so with several items, which is a task all of us are confronted with very frequently.

One has to toggle through all the unit options, which might consist of 7 categories.
Within each of these, we usually have to change at least two options.

That makes 14 clicks and changes for each instance.

What often happens then, is that users just change the `Length` category, leaving the family mixed, part imperial, part metric, for instance like this:

<img src="img/project_units_part_imperial_part_metric.png" alt="Project units part imperial part metric" title="Project units part imperial part metric" width="368"/>

## Solution

The solution is straight-forward: all unit settings seen by a user in a document in `ProjectUnits` are stored in a container class `DisplayUnit`.

These settings can be read from one document through its `DisplayUnitSystem` and set to another with the `SetUnits` method.

The add-in take the source data from a source document, stores it, and sets it to all target documents. 
 
One could obviously alter this to read data from a text file or something more fancy &ndash; but why bother when you can use a Revit template?

Want to convert to metric?
Select the metric template of your choice and paste it where you want.

Want to convert to imperial? Same thing.

Imperial decimal to imperial fractional? Just like before.

The add-in requires a source file (like a template) from which to read the units; then, it will write those units to all Revit files inside a selected folder.

## Authors

- Dragos Turmac, Principal Engineer, Autodesk
- Bogdan Teodorescu, Team Manager, Autodesk
- Jeremy Tammik, [The Building Coder](http://thebuildingcoder.typepad.com), [ADN](http://www.autodesk.com/adn) [Open](http://www.autodesk.com/adnopen), [Autodesk Inc.](http://www.autodesk.com)


## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT).
Please see the [LICENSE](LICENSE) file for full details.

