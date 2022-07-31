<%@ Page Title="Mobile Data Solution" Language="C#" MasterPageFile="~/Main.Master"
    AutoEventWireup="true" CodeBehind="MobileDataSolution.aspx.cs" Inherits="BDI2Web.Download.MobileDataSolution" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="radT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MCP" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Download - BDI-2 Mobile Data Solution for Windows"
        SkinID="Title" />
    <br />
    <br />
    <div style="text-align: center">
        <asp:Button ID="DownloadButton" runat="server" Text="DOWNLOAD" OnClick="DownloadButton_Click" />
    </div>
    <br />
    <br />
    <div>
        <asp:Label ID="MDSInstallLabel" runat="server" />&nbsp;&nbsp;<a href="http://downloads.hmlt.hmco.com/Help/bdi2datamanager/index.htm#Download_MDS.htm"
            target="_blank">MDS Install Instructions</a>
    </div>
    <br />
    <br />
    <div class="text" style="text-align: center">
        <b>BDI-2 – Windows Solution – System Requirements</b></div>
    <radT:RadSplitter ID="rs" runat="server">
        <radT:RadPane Scrolling="Y" Height="450px" Width="750px" runat="server">
            <div class="text">
                <br />
                The minimum hardware requirements for <b>Windows XP (Home Edition & Professional)</b>
                are:
                <ul>
                    Pentium 300-megahertz (MHz) processor or faster (500 MHz is recommended)</ul>
                <ul>
                    512 megabytes (MB) of system memory
                </ul>
                <ul>
                    2-gigabyte (GB) of free hard disk space
                </ul>
                <ul>
                    Keyboard and a Microsoft Mouse or some other compatible pointing device
                </ul>
                <ul>
                    Video adapter and monitor with Super VGA (1024 x 600) or higher resolution
                </ul>
                <ul>
                    Internet access capability</ul>
                <ul>
                    Adobe® Acrobat® 7 or 8</ul>
                <p>
                    *Microsoft announced that it will no longer support Windows XP as of April 8, 2014.
                    Due to this, should customers continue using Windows XP after April 8, 2014, HMH
                    cannot guarantee that we will be able to provide satisfactory resolution to technical
                    issues that Windows XP customers may experience with our online products.
                </p>
                The minimum hardware requirements for <b>Windows Vista Home Basic</b> are:
                <ul>
                    800-megahertz (MHz) 32-bit (x86) processor or 800-MHz 64-bit (x64) processor
                </ul>
                <ul>
                    512 megabytes (MB) of system memory</ul>
                <ul>
                    2-gigabyte (GB) of free hard disk space
                </ul>
                <ul>
                    Keyboard and a Microsoft Mouse or some other compatible pointing device
                </ul>
                <ul>
                    A graphics processor that is DirectX 9 capable, with a minimum of 64mb of RAM</ul>
                <ul>
                    Video adapter and monitor with Super VGA (1024 x 600) or higher resolution</ul>
                <ul>
                    Internet access capability</ul>
                <ul>
                    Adobe® Acrobat® 7 or 8</ul>
                The minimum hardware requirements for <b>Windows Vista Ultimate</b> are:
                <ul>
                    1-gigahertz (GHz) 32-bit (x86) processor or 1-GHz 64-bit (x64) processor
                </ul>
                <ul>
                    1 GB of system memory
                </ul>
                <ul>
                    Keyboard and a Microsoft Mouse or some other compatible pointing device</ul>
                <ul>
                    Support for DirectX 9 graphics with a WDDM driver, 128 MB of graphics memory (minimum),
                    Pixel Shader 2.0 and 32 bits per pixel.</ul>
                <ul>
                    2-gigabyte (GB) of free hard disk space
                </ul>
                <ul>
                    Internet access capability
                </ul>
                <ul>
                    Adobe® Acrobat® 7 or 8</ul>
                The minimum hardware requirements for <b>Windows 7 (Starter, Ultimate and Home Premium)</b>
                are:
                <ul>
                    1 gigahertz (GHz) or faster 32-bit (x86) or 64-bit (x64) processor
                </ul>
                <ul>
                    1 gigabyte (GB) RAM (32-bit) or 2 GB RAM (64-bit)
                </ul>
                <ul>
                    Keyboard and a Microsoft Mouse or some other compatible pointing device</ul>
                <ul>
                    DirectX 9 graphics device with WDDM 1.0 or higher driver
                </ul>
                <ul>
                    2-gigabyte (GB) of free hard disk space
                </ul>
                <ul>
                    Internet access capability
                </ul>
                <ul>
                    Adobe® Acrobat® 7 or 8</ul>
                The minimum hardware requirements for <b>Windows 8</b>
                are:
                <ul>
                    Processor: 1 gigahertz (GHz) or faster 
                </ul>
                <ul>
                    RAM: 1 gigabyte (GB) (32-bit) or 2 GB (64-bit) 
                </ul>
                <ul>
                    Hard disk space: 16 GB (32-bit) or 20 GB (64-bit)</ul>
                <ul>
                    Graphics card: Microsoft DirectX 9 graphics device with WDDM driver
                </ul>
                <ul>
                    Adobe® Acrobat® DC 
                </ul>
                The minimum hardware requirements for <b>Windows 10</b>
                are:
                <ul>
                    Processor: 1 gigahertz (GHz) or faster. 
                </ul>
                <ul>
                    RAM: 1 gigabyte (GB) (32-bit) or 2 GB (64-bit) 
                </ul>
                <ul>
                    Free hard disk space: 16 GB.</ul>
                <ul>
                    Graphics card: Microsoft DirectX 9 graphics device with WDDM driver. 
                </ul>
                <ul>A Microsoft account and Internet access. </ul>
                <ul>
                    Adobe® Acrobat® DC  
                </ul>
            </div>
        </radT:RadPane>
    </radT:RadSplitter>
</asp:Content>
