﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAmLichVN.Models
{
    public class VietCalendar
    {
        public static double PI = Math.PI;

        public static int jdFromDate(int dd, int mm, int yy)
        {
            int a = ((14 - mm)
                        / 12);
            int y = (yy + (4800 - a));
            int m = (mm
                        + ((12 * a)
                        - 3));
            int jd = (dd
                        + ((((153 * m)
                        + 2)
                        / 5)
                        + ((365 * y)
                        + (((y / 4)
                        - (y / 100))
                        + ((y / 400)
                        - 32045)))));
            if ((jd < 2299161))
            {
                jd = (dd
                            + ((((153 * m)
                            + 2)
                            / 5)
                            + ((365 * y)
                            + ((y / 4)
                            - 32083))));
            }

            // jd = jd - 1721425;
            return jd;
        }

        public static int[] jdToDate(int jd)
        {
            int c;
            int a;
            int b;
            if ((jd > 2299160))
            {
                //  After 5/10/1582, Gregorian calendar
                a = (jd + 32044);
                b = (((4 * a)
                            + 3)
                            / 146097);
                c = (a
                            - ((b * 146097)
                            / 4));
            }
            else
            {
                b = 0;
                c = (jd + 32082);
            }

            int d = (((4 * c)
                        + 3)
                        / 1461);
            int e = (c
                        - ((1461 * d)
                        / 4));
            int m = (((5 * e)
                        + 2)
                        / 153);
            int day = ((e
                        - (((153 * m)
                        + 2)
                        / 5))
                        + 1);
            int month = (m + (3 - (12
                        * (m / 10))));
            int year = ((b * 100)
                        + ((d - 4800)
                        + (m / 10)));
            return new int[] {
                day,
                month,
                year};
        }

        public static double SunLongitude(double jdn)
        {
            // return CC2K.sunLongitude(jdn);
            return VietCalendar.SunLongitudeAA98(jdn);
        }

        public static double SunLongitudeAA98(double jdn)
        {
            double T = ((jdn - 2451545)
                        / 36525);
            //  Time in Julian centuries from 2000-01-01 12:00:00 GMT
            double T2 = (T * T);
            double dr = (PI / 180);
            //  degree to radian
            double M = (357.5291
                        + ((35999.0503 * T)
                        - ((0.0001559 * T2) - (4.8E-07
                        * (T * T2)))));
            //  mean anomaly, degree
            double L0 = (280.46645
                        + ((36000.76983 * T) + (0.0003032 * T2)));
            //  mean longitude, degree
            double DL = ((1.9146
                        - ((0.004817 * T) - (1.4E-05 * T2)))
                        * Math.Sin((dr * M)));
            DL = (DL
                        + (((0.019993 - (0.000101 * T))
                        * Math.Sin((dr * (2 * M)))) + (0.00029 * Math.Sin((dr * (3 * M))))));
            double L = (L0 + DL);
            //  true longitude, degree
            L = (L - (360 * VietCalendar.INT((L / 360))));
            //  Normalize to (0, 360)
            return L;
        }

        public static double NewMoon(int k)
        {
            // return CC2K.newMoonTime(k);
            return VietCalendar.NewMoonAA98(k);
        }

        public static double NewMoonAA98(int k)
        {
            double T = (k / 1236.85);
            //  Time in Julian centuries from 1900 January 0.5
            double T2 = (T * T);
            double T3 = (T2 * T);
            double dr = (PI / 180);
            double Jd1 = (2415020.75933
                        + ((29.53058868 * k)
                        + ((0.0001178 * T2) - (1.55E-07 * T3))));
            Jd1 = (Jd1 + (0.00033 * Math.Sin(((166.56
                            + ((132.87 * T) - (0.009173 * T2)))
                            * dr))));
            //  Mean new moon
            double M = (359.2242
                        + ((29.10535608 * k)
                        - ((3.33E-05 * T2) - (3.47E-06 * T3))));
            //  Sun's mean anomaly
            double Mpr = (306.0253
                        + ((385.81691806 * k)
                        + ((0.0107306 * T2) + (1.236E-05 * T3))));
            //  Moon's mean anomaly
            double F = (21.2964
                        + ((390.67050646 * k)
                        - ((0.0016528 * T2) - (2.39E-06 * T3))));
            //  Moon's argument of latitude
            double C1 = (((0.1734 - (0.000393 * T))
                        * Math.Sin((M * dr))) + (0.0021 * Math.Sin((2
                            * (dr * M)))));
            C1 = ((C1 - (0.4068 * Math.Sin((Mpr * dr)))) + (0.0161 * Math.Sin((dr * (2 * Mpr)))));
            C1 = (C1 - (0.0004 * Math.Sin((dr * (3 * Mpr)))));
            C1 = (C1
                        + ((0.0104 * Math.Sin((dr * (2 * F)))) - (0.0051 * Math.Sin((dr
                            * (M + Mpr))))));
            C1 = ((C1 - (0.0074 * Math.Sin((dr
                            * (M - Mpr))))) + (0.0004 * Math.Sin((dr
                            * ((2 * F)
                            + M)))));
            C1 = (C1
                        - ((0.0004 * Math.Sin((dr
                            * ((2 * F)
                            - M)))) - (0.0006 * Math.Sin((dr
                            * ((2 * F)
                            + Mpr))))));
            C1 = (C1
                        + ((0.001 * Math.Sin((dr
                            * ((2 * F)
                            - Mpr)))) + (0.0005 * Math.Sin((dr
                            * ((2 * Mpr)
                            + M))))));
            double deltat;
            if ((T < -11))
            {
                deltat = (0.001
                            + ((0.000839 * T)
                            + ((0.0002261 * T2)
                            - ((8.45E-06 * T3) - (8.1E-08
                            * (T * T3))))));
            }
            else
            {
                deltat = (-0.000278
                            + ((0.000265 * T) + (0.000262 * T2)));
            }

            double JdNew = (Jd1
                        + (C1 - deltat));
            return JdNew;
        }

        public static int INT(double d)
        {
            return ((int)(Math.Floor(d)));
        }

        public static double getSunLongitude(int dayNumber, double timeZone)
        {
            return VietCalendar.SunLongitude((dayNumber - (0.5
                            - (timeZone / 24))));
        }

        public static int getNewMoonDay(int k, double timeZone)
        {
            double jd = VietCalendar.NewMoon(k);
            return VietCalendar.INT((jd + (0.5
                            + (timeZone / 24))));
        }

        public static int getLunarMonth11(int yy, double timeZone)
        {
            double off = (VietCalendar.jdFromDate(31, 12, yy) - 2415021.0769986948);
            int k = VietCalendar.INT((off / 29.530588853));
            int nm = VietCalendar.getNewMoonDay(k, timeZone);
            int sunLong = VietCalendar.INT((VietCalendar.getSunLongitude(nm, timeZone) / 30));
            if ((sunLong >= 9))
            {
                nm = VietCalendar.getNewMoonDay((k - 1), timeZone);
            }

            return nm;
        }

        public static int getLeapMonthOffset(int a11, double timeZone)
        {
            int k = VietCalendar.INT((0.5
                            + ((a11 - 2415021.0769986948)
                            / 29.530588853)));
            int last;
            //  Month 11 contains point of sun longutide 3*PI/2 (December solstice)
            int i = 1;
            //  We start with the month following lunar month 11
            int arc = VietCalendar.INT((VietCalendar.getSunLongitude(VietCalendar.getNewMoonDay((k + i), timeZone), timeZone) / 30));
            do
            {
                last = arc;
                i++;
                arc = VietCalendar.INT((VietCalendar.getSunLongitude(VietCalendar.getNewMoonDay((k + i), timeZone), timeZone) / 30));
            } while (((arc != last)
                        && (i < 14)));

            return (i - 1);
        }
        // Duong lich sang Am lich 
        public static int[] convertSolar2Lunar(int dd, int mm, int yy, double timeZone)
        {
            int lunarLeap;
            int lunarDay;
            int lunarMonth;
            int lunarYear;
            int dayNumber = VietCalendar.jdFromDate(dd, mm, yy);
            int k = VietCalendar.INT(((dayNumber - 2415021.0769986948)
                            / 29.530588853));
            int monthStart = VietCalendar.getNewMoonDay((k + 1), timeZone);
            if ((monthStart > dayNumber))
            {
                monthStart = VietCalendar.getNewMoonDay(k, timeZone);
            }

            int a11 = VietCalendar.getLunarMonth11(yy, timeZone);
            int b11 = a11;
            if ((a11 >= monthStart))
            {
                lunarYear = yy;
                a11 = VietCalendar.getLunarMonth11((yy - 1), timeZone);
            }
            else
            {
                lunarYear = (yy + 1);
                b11 = VietCalendar.getLunarMonth11((yy + 1), timeZone);
            }

            lunarDay = ((dayNumber - monthStart)
                        + 1);
            int diff = VietCalendar.INT(((monthStart - a11)
                            / 29));
            lunarLeap = 0;
            lunarMonth = (diff + 11);
            if (((b11 - a11)
                        > 365))
            {
                int leapMonthDiff = VietCalendar.getLeapMonthOffset(a11, timeZone);
                if ((diff >= leapMonthDiff))
                {
                    lunarMonth = (diff + 10);
                    if ((diff == leapMonthDiff))
                    {
                        lunarLeap = 1;
                    }

                }

            }

            if ((lunarMonth > 12))
            {
                lunarMonth = (lunarMonth - 12);
            }

            if (((lunarMonth >= 11)
                        && (diff < 4)))
            {
                lunarYear--;
            }

            return new int[] {
                lunarDay,
                lunarMonth,
                lunarYear,
                lunarLeap};
        }
        // Am lich sang Duong lich
        public static int[] convertLunar2Solar(int lunarDay, int lunarMonth, int lunarYear, int lunarLeap, double timeZone)
        {
            int b11;
            int a11;
            if ((lunarMonth < 11))
            {
                a11 = VietCalendar.getLunarMonth11((lunarYear - 1), timeZone);
                b11 = VietCalendar.getLunarMonth11(lunarYear, timeZone);
            }
            else
            {
                a11 = VietCalendar.getLunarMonth11(lunarYear, timeZone);
                b11 = VietCalendar.getLunarMonth11((lunarYear + 1), timeZone);
            }

            int k = VietCalendar.INT((0.5
                            + ((a11 - 2415021.0769986948)
                            / 29.530588853)));
            int off = (lunarMonth - 11);
            if ((off < 0))
            {
                off += 12;
            }

            if (((b11 - a11)
                        > 365))
            {
                int leapOff = VietCalendar.getLeapMonthOffset(a11, timeZone);
                int leapMonth = (leapOff - 2);
                if ((leapMonth < 0))
                {
                    leapMonth += 12;
                }

                if (((lunarLeap != 0)
                            && (lunarMonth != leapMonth)))
                {
                    // System.out.println("Invalid input!");
                    return new int[] {
                        0,
                        0,
                        0};
                }
                else if (((lunarLeap != 0)
                            || (off >= leapOff)))
                {
                    off++;
                }

            }

            int monthStart = VietCalendar.getNewMoonDay((k + off), timeZone);
            return VietCalendar.jdToDate((monthStart
                            + (lunarDay - 1)));
        }
    }
}