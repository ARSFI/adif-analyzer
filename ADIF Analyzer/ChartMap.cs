using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADIF_Analyzer
{
    public partial class ChartMap : Form
    {
        private int intTimerCountdown = 1;
        private string MapName;
        private Bitmap MapImage;

        private double ZoomFactor = 1.0;
        private double MapPixelLatitude0 = 873;             // Y pixel for 0 latitude
        private double MapPixelLongitude0 = 1029;           // X pixel for 0 longitude
        private double MapScaleLatitude = 325.016491;        // Map scale for latitude
        private double MapScaleLongitude = 5.684848;        // Pixels per degree of longitude
        private int intRMSMarkerSize = 6;
        private int intMarkerSize = 5;
        private int intSelectedMarkerSize = 8;
        private int intMouseX = 0;
        private int intMouseY = 0;
        private bool blnPaintBusy = false;
        private AdifRecord objSelectedEntry = null;
        List<AdifRecord> lstRec = null;

        public ChartMap()
        {
            InitializeComponent();
        }

        /*------------------------------------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartMap_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Map", this, 800, 500);
            ZoomFactor = Globals.objINIFile.GetDouble("Chart Map", "ZoomFactor", 1.0);
            pnlMap.AutoScroll = true;
            LatitudeLongitude objCenter = new LatitudeLongitude();
            objCenter.dblLatitude = Globals.objINIFile.GetDouble("Chart Map", "Center Latitude", 360);
            objCenter.dblLongitude = Globals.objINIFile.GetDouble("Chart Map", "Center Longitude", 360);
            /* File specification for map file */
            MapName = Globals.strExecutionDirectory + "WorldMap.jpg";
            /* Picture Box Settings */
            pbMapImage.SizeMode = PictureBoxSizeMode.AutoSize;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            Image image = null;
            try
            {
                image = Image.FromFile(MapName, true);
            }
            catch
            {
                Globals.Exceptions("Unable to load map image: " + MapName);
                MessageBox.Show("Unable to load map image file: " + MapName);
                return;
            }
            MapImage = new Bitmap(image);
            /* Initialize position */
            ZoomImage(1.0);
            /* Center the map */
            if (objCenter.dblLongitude >= 181 || objCenter.dblLatitude >= 181)
            {
                /* Put the RMS at the center of the map */
                objCenter = Globals.objStationLocation;
            }
            SetCenter(objCenter);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*--------------------------------------------------------------------------------
         * Get the latitude/longitude at the center of the display.
         */
        private LatitudeLongitude GetCenter()
        {
            LatitudeLongitude objLoc = new LatitudeLongitude();
            int X = pnlMap.HorizontalScroll.Value + pnlMap.Width / 2;
            int Y = pnlMap.VerticalScroll.Value + pnlMap.Height / 2;
            objLoc.dblLatitude = YtoLatitude(Y);
            objLoc.dblLongitude = XtoLongitude(X);
            return objLoc;
        }

        /*----------------------------------------------------------------------------------------
         * Try to scroll to center the map over the specified latitude/longitude.
         */
        private void SetCenter(LatitudeLongitude objLoc)
        {
            int X = Math.Max(0, LongitudeToX(objLoc.dblLongitude) - pnlMap.Width / 2);
            int Y = Math.Max(0, LatitudeToY(objLoc.dblLatitude) - pnlMap.Height / 2);
            int intNewX = Math.Min(pnlMap.HorizontalScroll.Maximum, X);
            pnlMap.HorizontalScroll.Value = intNewX;
            pnlMap.HorizontalScroll.Value = intNewX;
            int intNewY = Math.Min(pnlMap.VerticalScroll.Maximum, Y);
            pnlMap.VerticalScroll.Value = intNewY;
            pnlMap.VerticalScroll.Value = intNewY;
            return;
        }

        /*---------------------------------------------------------------------------------
         * This form is being closed.
         */
        private void ChartMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.objINIFile.WriteDouble("Chart Map", "ZoomFactor", ZoomFactor);
            LatitudeLongitude objCenter = GetCenter();
            Globals.objINIFile.WriteDouble("Chart Map", "Center Latitude", objCenter.dblLatitude);
            Globals.objINIFile.WriteDouble("Chart Map", "Center Longitude", objCenter.dblLongitude);
            Globals.SaveFormPosition("Chart Map", this);
            Globals.objMain.ChartMapClosed();
            return;
        }

        /*------------------------------------------------------------------------
         * A timer tick occurred.
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (intTimerCountdown > 0 && --intTimerCountdown <= 0)
            {
                intTimerCountdown = 0;
                DrawMap();
            }
            timer1.Enabled = true;
            return;
        }

        /*--------------------------------------------------------------------------
         * New data has arrived.  Update the table.
         */
        public void UpdateChart()
        {
            intTimerCountdown = 1;
            return;
        }

        /*------------------------------------------------------------------------------
         * Fill the table with entries.
         */
        private void DrawMap()
        {
            this.Cursor = Cursors.WaitCursor;
            /*
             * Get a list of recent ADIF records.
             */
            lstRec = Globals.objAdif.GetUniqueLocationAdifRecords(Globals.intMaxMapContacts);
            this.Cursor = Cursors.Default;
            /*
             * Force the map to be painted.
             */
            pbMapImage.Invalidate();
            return;
        }
        /*------------------------------------------------------------------------
         * The mouse has been clicked.
         */
        private void pbMapImage_MouseDown(object sender, MouseEventArgs e)
        {
            /*
             * If they clicked on a connection point, display its information.
             */
            AdifRecord objRec = FindClosestConnection(intMouseX, intMouseY);
            objSelectedEntry = objRec;
            if (objRec != null)
            {
                txtCallsign.Text = objRec.strConnectedCallsign;
                if (Globals.blnTimeZoneUTC)
                {
                    txtDateTime.Text = Globals.FormatDateTime(objRec.dttQsoStart);
                }
                else
                {
                    DateTime dttQsoStart = objRec.dttQsoStart;
                    if (!Globals.blnTimeZoneUTC) dttQsoStart = dttQsoStart.ToLocalTime();
                    txtDateTime.Text = Globals.FormatDateTime(dttQsoStart);
                }
            }
            else
            {
                txtCallsign.Text = "";
                txtDateTime.Text = "";
            }
            /*
             * Display current latitude/longitude.
             */
            double Latitude = YtoLatitude(e.Y);
            double Longitude = XtoLongitude(e.X);
            txtLatitude.Text = e.Y.ToString() + "/" + FormatPosition(Latitude, false);
            txtLongitude.Text = e.X.ToString() + "/" + FormatPosition(Longitude, true);
            /*
             * Repaint map to mark selected RMS.
             */
            pbMapImage.Invalidate();
            return;
        }

        /*-----------------------------------------------------------------
         * The mouse has moved.
         */
        private void pbMapImage_MouseMove(object sender, MouseEventArgs e)
        {
            intMouseX = e.X;
            intMouseY = e.Y;
            /* Display current latitude and longitude. */
            double Latitude = YtoLatitude(e.Y);
            double Longitude = XtoLongitude(e.X);
            txtLatitude.Text = FormatPosition(Latitude, false);
            txtLongitude.Text = FormatPosition(Longitude, true);
            return;
        }

        /*--------------------------------------------------------------------
         * The mouse is hovering.
         */
        private void pbMapImage_MouseHover(object sender, EventArgs e)
        {
#if false
            AdifRecord objRec = FindClosestConnection(intMouseX, intMouseY);
            if (objRec != null)
            {
                txtCallsign.Text = objRec.strConnectedCallsign;
                txtDateTime.Text = Globals.FormatDateTime(objRec.dttQsoStart);
            }
            else
            {
                txtCallsign.Text = "";
                txtDateTime.Text = "";
            }
            txtCallsign.Invalidate();
            txtDateTime.Invalidate();
#endif
            return;
        }

        /*-------------------------------------------------------------------
         * Find the AdifRecord that's closest to the specified point.
         */
        private AdifRecord FindClosestConnection(int X, int Y)
        {
            double MaxDistance = 2;
            LatitudeLongitude objLoc = new LatitudeLongitude();
            objLoc.dblLatitude = YtoLatitude(Y);
            objLoc.dblLongitude = XtoLongitude(X);
            AdifRecord objBest = null;
            double Closest = 1E20;
            foreach(AdifRecord objRec in lstRec)
            {
                double Ydist = objLoc.dblLatitude - objRec.Location.dblLatitude;
                double Xdist = objLoc.dblLongitude - objRec.Location.dblLongitude;
                double Distance = (Xdist * Xdist) + (Ydist * Ydist);
                if (Distance < MaxDistance)
                {
                    if (objBest == null || Distance < Closest)
                    {
                        Closest = Distance;
                        objBest = objRec;
                    }
                }
            }
            return objBest;
        }

        /*--------------------------------------------------------------------
         * Paint the image.
         */
        private void pbMapImage_Paint(object sender, PaintEventArgs e)
        {
            /*
             *  Create an image to work with.
             */
            if (blnPaintBusy) return;
            blnPaintBusy = true;
            Bitmap image = new Bitmap(pbMapImage.Image);
            Graphics g = Graphics.FromImage(image);
            /*
             * Mark each connection.
             */
            if (lstRec != null)
            {
                foreach (AdifRecord objRec in lstRec)
                {
                    DrawConnection(g, objRec);
                }
            }
            /*
             * Mark our position on the map.
             */
            DrawRMSMarker(g);
            /*
             * Mark the selected point (if any).
             */
            if (objSelectedEntry != null)
            {
                MarkSelectedEntry(g, objSelectedEntry);
            }
            /*
             *  Paint the map with the markers over it.
             */
            e.Graphics.DrawImage(image, 0, 0);
            /*
             * Finished
             */
            g.Dispose();
            image.Dispose();
            image = null;
            blnPaintBusy = false;
            return;
        }

        /*------------------------------------------------------------------------------
         * Put a marker at the location of the RMS.
         */
        private void DrawRMSMarker(Graphics g)
        {
            Pen penCircle = new Pen(Color.GreenYellow, 2);
            Pen penCrosshairs = new Pen(Color.FromArgb(200, 120, 0, 0), 1);
            int X = LongitudeToX(Globals.objStationLocation.dblLongitude);
            int Y = LatitudeToY(Globals.objStationLocation.dblLatitude);
            int Xp = Math.Max(0, (X - intRMSMarkerSize / 2));
            int Yp = Math.Max(0, (Y - intRMSMarkerSize / 2));
            g.DrawEllipse(penCircle, Xp, Yp, intRMSMarkerSize, intRMSMarkerSize);
            /* Draw an inner circle with a light color */
            int intInnerMarkerSize = intRMSMarkerSize / 4;
            Xp = Math.Max(0, (X - intInnerMarkerSize / 2));
            Yp = Math.Max(0, (Y - intInnerMarkerSize / 2));
            Pen penInnerCircle = new Pen(Color.Red, 1);
            g.DrawEllipse(penInnerCircle, Xp, Yp, intInnerMarkerSize, intInnerMarkerSize);
#if false
            int X = LongitudeToX(Globals.objStationLocation.dblLongitude);
            int Y = LatitudeToY(Globals.objStationLocation.dblLatitude);
            Pen penCircle = new Pen(Color.FromArgb(200, 120, 0, 0), 2);
            Pen penCrosshairs = new Pen(Color.FromArgb(200, 120, 0, 0), 1);
            if (X < 0) X = 0;
            if (Y < 0) Y = 0;
            if (intRMSMarkerSize < 1) intRMSMarkerSize = 1;
            Rectangle rectCircle = new Rectangle(X, Y, intRMSMarkerSize, intRMSMarkerSize);
            int Xp = (int)(X - intRMSMarkerSize / 2);
            int Yp = (int)(Y - intRMSMarkerSize / 2);
            if (Xp < 0) Xp = 0;
            if (Yp < 0) Yp = 0;
            g.DrawEllipse(penCircle, Xp, Yp, intRMSMarkerSize, intRMSMarkerSize);
            Xp = (int)(X + 1 - intRMSMarkerSize / 2);
            Yp = (int)(X - 1 + intRMSMarkerSize / 2);
            if (Xp < 0) Xp = 0;
            if (Yp < 0) Yp = 0;
            g.DrawLine(penCrosshairs, Xp, Y, Yp, Y);
            Xp = (int)(Y + 1 - intRMSMarkerSize / 2);
            Yp = (int)(Y - 1 + intRMSMarkerSize / 2);
            if (Xp < 0) Xp = 0;
            if (Yp < 0) Yp = 0;
            g.DrawLine(penCrosshairs, X, Xp, X, Yp);
#endif
            return;
        }

        /*--------------------------------------------------------------------------------
         * Draw a point where a connection was.
         */
        private void DrawConnection(Graphics g, AdifRecord objRec)
        {
            /* Convert latitude/longitude of point to pixels */
            int X = LongitudeToX(objRec.Location.dblLongitude);
            int Y = LatitudeToY(objRec.Location.dblLatitude);
            /* Draw a blue outter circle */
            Pen penCircle = new Pen(Color.FromArgb(200, 10, 10, 200), 2);
            Pen penCrosshairs = new Pen(Color.FromArgb(200, 120, 0, 0), 1);
            if (X < 0) X = 0;
            if (Y < 0) Y = 0;
            if (intMarkerSize < 1) intMarkerSize = 1;
            int Xp = Math.Max(0, (X - intMarkerSize / 2));
            int Yp = Math.Max(0, (Y - intMarkerSize / 2));
            g.DrawEllipse(penCircle, Xp, Yp, intMarkerSize, intMarkerSize);
            /* Draw an inner circle with a light color */
            int intInnerMarkerSize = intMarkerSize / 4;
            Xp = Math.Max(0, (X - intInnerMarkerSize / 2));
            Yp = Math.Max(0, (Y - intInnerMarkerSize / 2));
            Pen penInnerCircle = new Pen(Color.Yellow, 1);
            g.DrawEllipse(penInnerCircle, Xp, Yp, intInnerMarkerSize, intInnerMarkerSize);
            return;
        }

        /*------------------------------------------------------------------------
         * Mark a selected entry.
         */
        private void MarkSelectedEntry(Graphics g, AdifRecord objSelected)
        {
            /*
             * See if this entry is still in the list.
             */
            AdifRecord objEntry = null;
            foreach (AdifRecord objRec in lstRec)
            {
                if (objSelected.strConnectedCallsign == objRec.strConnectedCallsign &&
                    objSelected.strGridSquare == objRec.strGridSquare)
                {
                    objEntry = objRec;
                    break;
                }
            }
            if (objEntry == null)
            {
                /* Can't find the selected entry */
                return;
            }
            /*
             * Draw a marker on the selected entry.
             */
            /* Convert latitude/longitude of point to pixels */
            int X = LongitudeToX(objEntry.Location.dblLongitude);
            int Y = LatitudeToY(objEntry.Location.dblLatitude);
            int Xp = Math.Max(0, (X - intSelectedMarkerSize / 2));
            int Yp = Math.Max(0, (Y - intSelectedMarkerSize / 2));
            Pen penInnerCircle = new Pen(Color.Gold, 1);
            g.DrawEllipse(penInnerCircle, Xp, Yp, intSelectedMarkerSize, intSelectedMarkerSize);
            return;
        }

        /*----------------------------------------------------------------------
         * Zoom-In button was clicked.
         */
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomImage(1.33333334);
            return;
        }

        /*--------------------------------------------------------------------
         * Zoom-Out button was clicked.
         */
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomImage(0.75);
            return;
        }

        /*-----------------------------------------------------------------------
         * Zoom the image.
         */
        private void ZoomImage(double ZoomChange)
        {
            /* Save the current center position */
            LatitudeLongitude objCenter = GetCenter();
            /* Alter the zoom factor */
            double ZoomValue = ZoomFactor * ZoomChange;
            if (ZoomValue > 3.5)
            {
                ZoomValue = 3.5;
            }
            else if (ZoomValue < 0.4)
            {
                ZoomValue = 0.4;
            }
            /* Create a new image based on the zoom parameters we require */
            Bitmap zoomImage = new Bitmap(MapImage, (Convert.ToInt32(MapImage.Width * ZoomValue)), (Convert.ToInt32(MapImage.Height * ZoomValue)));
            /* Create a new graphics object based on the new image */
            Graphics converted = Graphics.FromImage(zoomImage);
            /* Use interpolation to clean up the image */
            converted.InterpolationMode = InterpolationMode.HighQualityBicubic;
            /* Clear out the original image */
            pbMapImage.Image = null;
            /* Display the new "zoomed" image */
            pbMapImage.Image = zoomImage;
            /* Save the new scale */
            ZoomFactor = ZoomValue;
            /* Fix the scroll position */
            SetCenter(objCenter);
            pbMapImage.Invalidate();
            return;
        }

        /*--------------------------------------------------------------------------------
         * Convert a longitude to the corresponding X pixel positon.
         */
        private int LongitudeToX(double Longitude)
        {
            double dblX = ZoomFactor * ((Longitude * MapScaleLongitude) + MapPixelLongitude0);
            if (dblX < 0.0)
            {
                Longitude = 360.0 - Longitude;
                dblX = ZoomFactor * ((Longitude * MapScaleLongitude) + MapPixelLongitude0);
            }
            return (int)(0.5 + dblX);
        }

        /*------------------------------------------------------------------------------------
         * Convert an X pixel position to the corresponding longitude.
         */
        private double XtoLongitude(int X)
        {
            double dblLongitude = ((X / ZoomFactor) - MapPixelLongitude0) / MapScaleLongitude;
            if (dblLongitude > 180.0)
            {
                dblLongitude = dblLongitude - 360.0;
            }
            else if (dblLongitude < -180.0)
            {
                dblLongitude = dblLongitude + 360.0;
            }
            return dblLongitude;
        }

        /*--------------------------------------------------------------------------
         * Convert a latitude (in degrees) to the corresponding Y pixel value.
         */
        private int LatitudeToY(double Latitude)
        {
            double LatRadians = DegreesToRadians(Latitude);
            double dblY = ZoomFactor * (MapPixelLatitude0 - (ASinh(Math.Tan(LatRadians)) * MapScaleLatitude));
            return (int)(0.5 + dblY);
        }

        /*--------------------------------------------------------------------------
         * Convert a Y pixel position to the latitude in degrees.
         */
        private double YtoLatitude(int Y)
        {
            double Latitude = RadiansToDegrees(Math.Atan(Math.Sinh((MapPixelLatitude0 - Y / ZoomFactor) / MapScaleLatitude)));
            return Latitude;
        }

        /*-------------------------------------------------------------------------------------------------------------
         * Inverse (arc) hyperbolic sine.
         */
        double ASinh(double dblValue)
        {
            return Math.Log(dblValue + Math.Sqrt(dblValue * dblValue + 1));
        }

        /*----------------------------------------------------------------------------------
         * Convert an angle in degrees to radians.
         */
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        /*----------------------------------------------------------------------------
         * Convert an angle in radians to degrees.
         */
        private double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        /*------------------------------------------------------------------------
         * Format a latitude or longitude.
         */
        private string FormatPosition(double dblPosition, bool blnLongitude)
        {
            bool blnIsNegative;
            int intDegrees;
            double dblMinutes;
            string strValue;
            /*
             * Make the value positive.
             */
            if (dblPosition < 0.0)
            {
                blnIsNegative = true;
                dblPosition = -dblPosition;
            }
            else
            {
                blnIsNegative = false;
            }
            /*
             * Split into degrees and minutes.
             */
            intDegrees = (int)(Math.Truncate(dblPosition));
            dblMinutes = 60.0 * (dblPosition - intDegrees);
            /*
             * Format the return value.
             */
            strValue = intDegrees.ToString("000") + Convert.ToChar(176) + " " + dblMinutes.ToString("00") + "'";
            if (blnIsNegative)
            {

                if (blnLongitude)
                {
                    strValue += " W";
                }
                else
                {
                    strValue += " S";
                }
            }
            else
            {
                if (blnLongitude)
                {
                    strValue += " E";
                }
                else
                {
                    strValue += " N";
                }
            }
            return strValue;
        }
    }
}
