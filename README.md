© Copyright 2014 Mission Aviation Fellowship
Project   : Weather Web Application / Weather Update
Author    : Elliott Stoddard

This application downloads weather data from the Open Weather Map API 
(http://openweathermap.org/api). If the data suddenly stops working, 
please check the URL to see if the Open Weather Map project is still working. 

Most errors are logged to a text log file in the application directory. 
Read that file to find out what the problem is.

I choose to use Open Weather Map because it was the one service that had weather 
data for the most locations that MAF works in. If Open Weather Map is no longer 
working, feel free to alter the OpenWeatherMap part of this app to conform to 
another data provider. This will require an installation of Visual Studio 2013 
or above. The Express version should work. 

This app does more than just download the data for local caching. The source 
provides temperatures in Kelvin, and wind speed in meters per second. The app
converts the speeds into multiple formats. Not all of the formats are displayed
on the UI. Additional data about the location, such as time zone are added to 
the output. This data uses the standard Linux time zone strings. If there is a
time zone problem, please try updating the Moment.JS files before updating 
this application. http://momentjs.com/timezone/

Add, Remove, or Change a Location
If you just need to add, remove, or change a location, please look at the 
WeatherUpdateSettings.xml file. This is a standard XML data file. Please make
sure that all edits to this file are valid XML. If the application does not work
after you edit this file, and it worked before you edited it, the problem is most 
likely that the changes you made were not valid XML. Please work with a web 
developer to make the correct changes. To find the location ID for a location use 
the following URL and replace London with the location name: 
http://api.openweathermap.org/data/2.5/find?q=London&type=accurate&mode=xml

I recommend double checking that the location ID is for the correct city. When
I was first setting this up I accidentally used the weather data for a city in 
Portugal when I really wanted data for a city in Brazil. They both had the same 
name and spelling. So use the URL below, and replace the ID at the end of the URL
with the ID you got from the previous query.
http://openweathermap.org/city/5601933

Data Location
The location that the data file is saved to is also in the WeatherUpdateSettings.xml 
file. If the web directory for the application has been changed, you will need to 
change this value. The tag name for this value is saveDataPath. It should be on the 
third line of the settings file.

Web UI
The UI is written in HTML5, JavaScript and CSS3. It uses KnockoutJS to update the 
UI when the data changes. The date time and time zone data is calculated using 
Moment.JS. All of the graphics are vector graphics using the SVG format. The country
flag SVG files should be in the /i/flags/ directory with the name of the country as 
the file name. 

If you add a new location with a new country, you will have to find or draw a vector 
version of the country flag. I recommend looking at the Wikipedia page for the 
country. There should be a link to the image of the current flag, and there should 
be a vector version of the flag. If you want to draw it yourself, I recommend using 
the open source application Inkscape. It is a great tool to create and edit vector
graphics. 

The webUI files should be installed on the SharePoint server automatically by the 
installer application. The installer should also set up an update schedule for the
data. This is best set 2 minutes before the hour and half hour. The webUI updates
every hour and half hour.

On a page that you want to display the Weather Clock, go to the page in SharePoint
and click the 'EDIT' button in the upper right hand corner. Put the cursor in the 
location you want the Weather Clock to be displayed. Click the tab 'INSERT'. Select 
'Web Part', 'Media and Content', 'Content Editor'. This will add the content editor
at the cursor location. In the upper right hand corner of the 'Content Editor' hover over
the upper left hand corner. A down arrow and a check box will appear. Click the down 
arrow and select 'Edit Web Part'. Copy the line below and paste it in the 'Content Link' 
field.

../SiteAssets/scripts/wxClock.txt

Make any other changes needed to the 'Content Editor' menu and click 'OK'. The Weather 
Clock should now display on the page. 

There are several ways to filter the locations displayed. One way is to start with 
the full list and adjust the filter select lists to view the locations you want to 
see. The label 'Filter:' before the select lists is a link that will open a new window 
with your filter selections saved as an URL query string.

The hidden way to pass in a filter is to edit the page in SharePoint, and insert a 
'Web Part', 'Media and Content', 'Script Editor'. In the new 'Script Editor' hover over
the upper left hand corner. A down arrow and a check box will appear. Click the down 
arrow and select 'Edit Web Part'. 

Click on the 'EDIT SNIPPET' link that appears in the web part. If you do not see the 
link, change the 'Chrome State' in the menu on the right. Then click the 'Apply' button. 
The link should then appear. 

In the 'Embed' modal window paste the code below. The // marks designate the line as disabled.
Enable any of the lines you want to filter by. To do this remove the // from the beginning of the line.
Next change the filter value. For example "Nampa" can be replaced with "Juba", or "Oaxaca". 
The system only filters by one location, region, or organization at a time.

<script type="text/javascript">
   //var wxLocation = "Nampa";
   //var wxRegion = "Africa";
   //var wxOrg = "MAF-I";
</script>

After the filter has been set, click the 'Insert' button. Make sure that the 'Chrome Type' is 
set to 'None' to hide the box and title. In the upper right hand corner click the 'SAVE' button.
If you do not see the items you wanted to see there is an error with the filter script. The location
name may have been misspelled. Feel free to copy the spelling directly from the full list on the web page.
