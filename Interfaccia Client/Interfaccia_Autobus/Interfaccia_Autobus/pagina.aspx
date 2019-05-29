<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pagina.aspx.cs" Inherits="pagina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <header><!---login e menu--></header>
        <div id="side">
        <div><!---selezioe mezzo-->
            <asp:DropDownList ID="selectid" runat="server">
            </asp:DropDownList>
        </div>
        <asp:GridView ID="rilevazioni" runat="server"><!--reord-->
        </asp:GridView>
        </div>
        <div id="mappa">
            <!--mappa-->
        </div>
        <div id="stat">
            <!--statistiche-->
        </div>
    </form>
    <link rel = "stylesheet" type = "text/css" href = "StyleSheet.css" />
    <script src="http://www.openlayers.org/api/OpenLayers.js"></script>
    <script type="text/javascript">
	
// Posizione iniziale della mappa
    var lat=44.355;
    var lon=11.71;
    var zoom=13;
		
    function init() {
 
        map = new OpenLayers.Map ("map", {
            controls:[ 
			new OpenLayers.Control.Navigation(),
                       new OpenLayers.Control.PanZoomBar(),
                       new OpenLayers.Control.ScaleLine(),
                       new OpenLayers.Control.Permalink('permalink'),
                       new OpenLayers.Control.MousePosition(),                    
                       new OpenLayers.Control.Attribution()
				      ],
            projection: new OpenLayers.Projection("EPSG:900913"),
            displayProjection: new OpenLayers.Projection("EPSG:4326")
            } );
 
		var mapnik = new OpenLayers.Layer.OSM("OpenStreetMap (Mapnik)");
		
		map.addLayer(mapnik);
 
        var lonLat = new OpenLayers.LonLat( lon ,lat )
          .transform(
            new OpenLayers.Projection("EPSG:4326"), // transform from WGS 1984
            map.getProjectionObject() // to Spherical Mercator Projection
          );
			  
		map.setCenter (lonLat, zoom);
 
 }
 
</script>
</body>
</html>
