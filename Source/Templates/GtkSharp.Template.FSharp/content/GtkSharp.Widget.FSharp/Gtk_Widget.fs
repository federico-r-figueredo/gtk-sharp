namespace GtkNamespace

open Gtk

type Gtk_Widget (builder : Builder) =
    inherit Box(builder.GetRawOwnedObject("Gtk_Widget").Handle)

    new() = new Gtk_Widget(new Builder("Gtk_Widget.glade"))
