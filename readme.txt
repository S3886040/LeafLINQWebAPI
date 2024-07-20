*-----------------------------------------------------------------------------------------
* System Requirements
*-----------------------------------------------------------------------------------------
Access to a PC, Laptop, or Mobile Phone.
Must have Internet Access.
Current Browser installed. (Chrome, Edge, Safari, or Firefox)

*-----------------------------------------------------------------------------------------
* GitHub Repos
*-----------------------------------------------------------------------------------------
WebApp GitHub repo: https://github.com/ProgrammingProject2024SP1Team6/LeafLINQWeb
WebAPI GitHub repo: https://github.com/ProgrammingProject2024SP1Team6/LeafLINQWebAPI
DeviceAPI GitHub repo: https://github.com/ProgrammingProject2024SP1Team6/LeafLINQAPI
LeafLINQEmbedded GitHub repo: https://github.com/ProgrammingProject2024SP1Team6/LeafLINQEmbedded

*-----------------------------------------------------------------------------------------
* Visual Studio Version. If you are going to run WebApp or API locally.
*-----------------------------------------------------------------------------------------
WebAPI: version = net7.0
WebApp: version = net7.0 
DeviceAPI: version = net7.0 

*-----------------------------------------------------------------------------------------
* SDK version. If you are going to run the WebApp or API locally.
*-----------------------------------------------------------------------------------------
WebAPI: version = "7.0.306"
WebApp: version = "7.0.306"
DeviceAPI: version = "7.0.306"

*-----------------------------------------------------------------------------------------
* Source code directory paths sln for VS etc.
*-----------------------------------------------------------------------------------------
WebApp [User root directory]/LeafLINQWeb/LeafLINQWeb.sln     (To open project in Visual studio)
       [User root directory]/LeafLINQWeb/program.cs          (application start)
       [User root directory]/LeafLINQWeb/LeafLINQWeb.csproj  (nuget packages, system parameters, etc)

WebAPI [User root directory]/LeafLINQWebAPI/LeafLINQWebAPI.sln     (To open project in Visual studio)
       [User root directory]/LeafLINQWebAPI/program.cs             (application start)
       [User root directory]/LeafLINQWebAPI/LeafLINQWebAPI.csproj  (nuget packages, system parameters, etc)

DeviceAPI [User root directory]/LeafLINQAPI/LeafLINQAPI.sln     (To open project in Visual studio)
       	  [User root directory]/LeafLINQAPI/program.cs             (application start)
          [User root directory]/LeafLINQAPI/LeafLINQAPI.csproj  (nuget packages, system parameters, etc)

*-----------------------------------------------------------------------------------------
* LeafLINQ Hardware Setup
*-----------------------------------------------------------------------------------------

Welcome to the LeafLINQ System! This document provides detailed instructions on setting up and configuring your LeafLINQ Access Point and Plant Node devices. Follow these steps carefully to ensure a smooth installation and start monitoring your plants with our innovative technology.

Product Overview:
--------------------------------------------------------------------------------
The LeafLINQ system consists of two main components:
1. Access Point Node: Acts as a hub, connecting the Plant Nodes to the internet.
2. Plant Node: Monitors various environmental parameters around the plant using integrated sensors.

What’s in the Box:
--------------------------------------------------------------------------------
- 1 x Access Point Node
- 3 x Plant Nodes
- Power adapters
- Quick Start Guide
- WiFi Password Card

Installation Instructions:
--------------------------------------------------------------------------------
1. Power Setup:
   - Connect the Access Point Node and the Plant Node to their respective power adapters and plug into a power outlet.

2. Sensor Setup:
   - Insert the Soil Moisture Sensor into the soil of the plant you wish to monitor.

3. Connect to Access Point:
   - From your device (smartphone, tablet, or PC), search for the WiFi network broadcast by the Access Point Node.
   - Use the supplied password (found on the WiFi Password Card) to connect to this network.

4. Configuration:
   - Open a web browser and navigate to the following address: 192.168.4.1
   - You will see a configuration form where you need to enter your local WiFi network details (SSID and password).
   - Submit the form by clicking the 'Submit' button.

5. Finalising Setup:
   - The device will attempt to connect to your local WiFi network. Upon successful connection, the Green LED on the Access Point Node will light up.
   - If the Green LED does not light up, re-enter your WiFi details to ensure they are correct and try again.

6. Troubleshooting:
   - If you continue to experience issues with connecting to your local WiFi network, please restart the device and repeat the configuration steps.
   - If problems persist, contact our support team for assistance.

*-----------------------------------------------------------------------------------------
*Support:
*-----------------------------------------------------------------------------------------
* If you *encounter any issues during installation or have questions about your LeafLINQ 
* system, please contact *our support team.
*
* Email: s3255561@student.rmit.edu.au
*
* Thank you for choosing LeafLINQ. 
* We are committed to providing you with quality service and support.
*-----------------------------------------------------------------------------------------

*-----------------------------------------------------------------------------------------
* To run the Application (live site).
*-----------------------------------------------------------------------------------------
1. Open your default browser. 
2. Go to https://leaflinq.azurewebsites.net/

*-----------------------------------------------------------------------------------------
* THE BELOW SECTIONS ARE FOR ADVANCED USERS AND OR DEVELOPERS
*-----------------------------------------------------------------------------------------
* To download the application from Github. Using the windows command line editor.
*-----------------------------------------------------------------------------------------
1. Right-click the windows start button and select Terminal (Admin).
   A window's command line editor will start.
2. Navigate to the directory you want to clone the application to.
   eg: cd Code/
3. Type: git clone https://github.com/ProgrammingProject2024SP1Team6/LeafLINQWeb

   Note: You may need to download and install Git before running any of the above Git commands.

   To download and install Git go to https://git-scm.com/download/win and follow the instructions provided.

*-----------------------------------------------------------------------------------------
* To run the Application (locally - After obtaining from Github)
*-----------------------------------------------------------------------------------------
1. Open Visual Studio. 
2. Select Open Project or Solution 
3. Browse to folder [rootProjectLibrary]/LeafLINQWeb/
4. Select LeafLINQWeb.sln file.
5. Select Start without debugging option to run the application.
6. The application will open your default browser and enter the local address https://localhost:7094/Login
   If the site does not open open your browser manually go to the site manually by entering the address in 
   your browser https://localhost:7094/Login

*-----------------------------------------------------------------------------------------
* To download the application from Github. Using the windows command line editor.
*-----------------------------------------------------------------------------------------
1. Right-click the windows start button and select Terminal (Admin).
   A window's command line editor will start.
2. Navigate to the directory you want to clone the application to.
   eg: cd Code/
3. Type: git clone https://github.com/ProgrammingProject2024SP1Team6/LeafLINQWebAPI

   Note: You may need to download and install Git before running any of the above Git commands.

   To download and install Git go to https://git-scm.com/download/win and follow the instructions provided.

*-----------------------------------------------------------------------------------------
* To run the Application API (locally - After obtaining from Github)
*-----------------------------------------------------------------------------------------
1. Open Visual Studio. 
2. Select Open Project or Solution 
3. Browse to folder [rootProjectLibrary]/LeafLINQWebAPI/
4. Select LeafLINQWebAPI.sln file.
5. Select Start without debugging option to run the application.
6. The application will run in the background. All output is directed to a console editor. 
   https://localhost:7063
   https://localhost:5018
