namespace FlightPlanner.Constants;

public static class GlobalFormulas
{
    //DENSITY FUEL
    public static decimal DENSITY_FUEL_GAL = 6.695m; // 6.7 lbs per gallon
    
    //TAXI AND HOLDING FUEL
    public static int TAXI_HOLDING_FUEL = 200;
    public static int DIAMOND_DA40_TAXI_HOLDING_FUEL = 7;
    public static int DIAMOND_DA62_TAXI_HOLDING_FUEL = 6;
    public static int CESSNA_S172_TAXI_HOLDING_FUEL = 1;
    
    //CESSNA CITATION LONGITUDE - FUEL CONSUMPTION
    public static int CESSNACL_HA_AVERAGE_FUEL = 1850;
    public static int CESSNACL_HA_RESERVE_FUEL = 1365;
    
    public static int CESSNACL_MA_AVERAGE_FUEL = 2000;
    public static int CESSNACL_MA_RESERVE_FUEL = 1500;
    
    public static int CESSNACL_LA_AVERAGE_FUEL = 2200;
    public static int CESSNACL_LA_RESERVE_FUEL = 1650;
    
    //CESSNA CITATION LONGITUDE - CRUISE ALTITUDE
    public static int CESSNACL_HA_CRUISE_ALTITUDE = 45000;
    public static int CESSNACL_MA_CRUISE_ALTITUDE = 41000;
    public static int CESSNACL_LA_CRUISE_ALTITUDE = 34000;
    
    //CESSNA CITATION LONGITUDE - CRUISE SPEED
    public static int CESSNACL_HA_CRUISE_SPEED = 440;
    public static int CESSNACL_MA_CRUISE_SPEED = 455;
    public static int CESSNACL_LA_CRUISE_SPEED = 470;
    
    //EMBRAER E190 - FUEL CONSUMPTION
    public static int EMBRAER_E190_HA_AVERAGE_FUEL = 2600;
    public static int EMBRAER_E190_HA_RESERVE_FUEL = 1950;
    
    public static int EMBRAER_E190_MA_AVERAGE_FUEL = 2800;
    public static int EMBRAER_E190_MA_RESERVE_FUEL = 2100;
    
    public static int EMBRAER_E190_LA_AVERAGE_FUEL = 2900;
    public static int EMBRAER_E190_LA_RESERVE_FUEL = 2175;
    
    //EMBRAER E190 - CRUISE ALTITUDE
    public static int EMBRAER_E190_HA_CRUISE_ALTITUDE = 41000;
    public static int EMBRAER_E190_MA_CRUISE_ALTITUDE = 37000;
    public static int EMBRAER_E190_LA_CRUISE_ALTITUDE = 30000;
    
    //EMBRAER E190 - CRUISE SPEED
    public static int EMBRAER_E190_HA_CRUISE_SPEED = 447;
    public static int EMBRAER_E190_MA_CRUISE_SPEED = 465;
    public static int EMBRAER_E190_LA_CRUISE_SPEED = 445;
    
    //EMBRAER E195 - FUEL CONSUMPTION
    public static int EMBRAER_E195_HA_AVERAGE_FUEL = 2800;
    public static int EMBRAER_E195_HA_RESERVE_FUEL = 2100;
    
    public static int EMBRAER_E195_MA_AVERAGE_FUEL = 3000;
    public static int EMBRAER_E195_MA_RESERVE_FUEL = 2250;
    
    public static int EMBRAER_E195_LA_AVERAGE_FUEL = 3200;
    public static int EMBRAER_E195_LA_RESERVE_FUEL = 2400;
    
    //EMBRAER E195 - CRUISE ALTITUDE
    public static int EMBRAER_E195_HA_CRUISE_ALTITUDE = 39000;
    public static int EMBRAER_E195_MA_CRUISE_ALTITUDE = 36000;
    public static int EMBRAER_E195_LA_CRUISE_ALTITUDE = 31000;
    
    //EMBRAER E195 - CRUISE SPEED
    public static int EMBRAER_E195_HA_CRUISE_SPEED = 447;
    public static int EMBRAER_E195_MA_CRUISE_SPEED = 470;
    public static int EMBRAER_E195_LA_CRUISE_SPEED = 447;
    
    //AIRBUS A320NEO - FUEL CONSUMPTION
    public static int AIRBUS_A320NEO_HA_AVERAGE_FUEL = 4800;
    public static int AIRBUS_A320NEO_HA_RESERVE_FUEL = 3600;
    
    public static int AIRBUS_A320NEO_MA_AVERAGE_FUEL = 5200;
    public static int AIRBUS_A320NEO_MA_RESERVE_FUEL = 3900;
    
    public static int AIRBUS_A320NEO_LA_AVERAGE_FUEL = 5500;
    public static int AIRBUS_A320NEO_LA_RESERVE_FUEL = 4125;
    
    //AIRBUS A320NEO - CRUISE ALTITUDE
    public static int AIRBUS_A320NEO_HA_CRUISE_ALTITUDE = 39000;
    public static int AIRBUS_A320NEO_MA_CRUISE_ALTITUDE = 37000;
    public static int AIRBUS_A320NEO_LA_CRUISE_ALTITUDE = 33000;
    
    //AIRBUS A320NEO - CRUISE SPEED
    public static int AIRBUS_A320NEO_HA_CRUISE_SPEED = 480;
    public static int AIRBUS_A320NEO_MA_CRUISE_SPEED = 470;
    public static int AIRBUS_A320NEO_LA_CRUISE_SPEED = 450;
    
    //BOEING 737 MAX - FUEL CONSUMPTION
    public static int BOEING_737_MAX_HA_AVERAGE_FUEL = 4600;
    public static int BOEING_737_MAX_HA_RESERVE_FUEL = 3450;
    
    public static int BOEING_737_MAX_MA_AVERAGE_FUEL = 5000;
    public static int BOEING_737_MAX_MA_RESERVE_FUEL = 3750;
    
    public static int BOEING_737_MAX_LA_AVERAGE_FUEL = 5600;
    public static int BOEING_737_MAX_LA_RESERVE_FUEL = 4200;
    
    //BOEING 737 MAX - CRUISE ALTITUDE
    public static int BOEING_737_MAX_HA_CRUISE_ALTITUDE = 39000;
    public static int BOEING_737_MAX_MA_CRUISE_ALTITUDE = 35000;
    public static int BOEING_737_MAX_LA_CRUISE_ALTITUDE = 29000;
    
    //AIRBUS A320NEO - CRUISE SPEED
    public static int BOEING_737_MAX_HA_CRUISE_SPEED = 460;
    public static int BOEING_737_MAX_MA_CRUISE_SPEED = 450;
    public static int BOEING_737_MAX_LA_CRUISE_SPEED = 430;
    
    //CESSNA S172 - FUEL CONSUMPTION
    public static int CESSNA_S172_HA_AVERAGE_FUEL = 39;
    public static int CESSNA_S172_HA_RESERVE_FUEL = 48;
    
    public static int CESSNA_S172_MA_AVERAGE_FUEL = 45;
    public static int CESSNA_S172_MA_RESERVE_FUEL = 36;
    
    public static int CESSNA_S172_LA_AVERAGE_FUEL = 51;
    public static int CESSNA_S172_LA_RESERVE_FUEL = 24;
    
    //CESSNA S172 - CRUISE ALTITUDE
    public static int CESSNA_S172_HA_CRUISE_ALTITUDE = 10500;
    public static int CESSNA_S172_MA_CRUISE_ALTITUDE = 7500;
    public static int CESSNA_S172_LA_CRUISE_ALTITUDE = 3500;
    
    //CESSNA S172 - CRUISE SPEED
    public static int CESSNA_S172_HA_CRUISE_SPEED = 113;
    public static int CESSNA_S172_MA_CRUISE_SPEED = 118;
    public static int CESSNA_S172_LA_CRUISE_SPEED = 124;
    
    //CESSNA S172 - SPEED CONFIGURATIONS
    public static int CESSNA_S172_ASCENT_RATE = 75;
    public static int CESSNA_S172_DESCENT_RATE = 90;
    public static int CESSNA_S172_FINAL_APPROACH_SPEED = 65;
    public static int CESSNA_S172_DESCENT_VERTICAL_SPEED = 500;
    
    //DIAMOND DA40 NG - FUEL CONSUMPTION
    public static int DIAMOND_DA40_HA_AVERAGE_FUEL = 29;
    public static int DIAMOND_DA40_HA_RESERVE_FUEL = 44;
    
    public static int DIAMOND_DA40_MA_AVERAGE_FUEL = 32;
    public static int DIAMOND_DA40_MA_RESERVE_FUEL = 33;
    
    public static int DIAMOND_DA40_LA_AVERAGE_FUEL = 36;
    public static int DIAMOND_DA40_LA_RESERVE_FUEL = 22;
    
    //DIAMOND DA40 NG - CRUISE ALTITUDE
    public static int DIAMOND_DA40_HA_CRUISE_ALTITUDE = 12000;
    public static int DIAMOND_DA40_MA_CRUISE_ALTITUDE = 8000;
    public static int DIAMOND_DA40_LA_CRUISE_ALTITUDE = 4500;
    
    //DIAMOND DA40 NG - CRUISE SPEED
    public static int DIAMOND_DA40_HA_CRUISE_SPEED = 150;
    public static int DIAMOND_DA40_MA_CRUISE_SPEED = 145;
    public static int DIAMOND_DA40_LA_CRUISE_SPEED = 135;
    
    //DIAMOND DA40 NG - SPEED CONFIGURATIONS
    public static int DIAMOND_DA40_ASCENT_RATE = 90;
    public static int DIAMOND_DA40_DESCENT_RATE = 110;
    public static int DIAMOND_DA40_FINAL_APPROACH_SPEED = 75;
    public static int DIAMOND_DA40_DESCENT_VERTICAL_SPEED = 700;
    
    //DIAMOND DA62 - FUEL CONSUMPTION
    public static int DIAMOND_DA62_HA_AVERAGE_FUEL = 78;
    public static int DIAMOND_DA62_HA_RESERVE_FUEL = 75;
    
    public static int DIAMOND_DA62_MA_AVERAGE_FUEL = 75;
    public static int DIAMOND_DA62_MA_RESERVE_FUEL = 60;
    
    public static int DIAMOND_DA62_LA_AVERAGE_FUEL = 70;
    public static int DIAMOND_DA62_LA_RESERVE_FUEL = 45;
    
    //DIAMOND DA62 - CRUISE ALTITUDE
    public static int DIAMOND_DA62_HA_CRUISE_ALTITUDE = 14000;
    public static int DIAMOND_DA62_MA_CRUISE_ALTITUDE = 9000;
    public static int DIAMOND_DA62_LA_CRUISE_ALTITUDE = 6000;
    
    //DIAMOND DA62 - CRUISE SPEED
    public static int DIAMOND_DA62_HA_CRUISE_SPEED = 190;
    public static int DIAMOND_DA62_MA_CRUISE_SPEED = 180;
    public static int DIAMOND_DA62_LA_CRUISE_SPEED = 170;
    
    //DIAMOND DA62 - SPEED CONFIGURATIONS
    public static int DIAMOND_DA62_ASCENT_RATE = 110;
    public static int DIAMOND_DA62_DESCENT_RATE = 120;
    public static int DIAMOND_DA62_FINAL_APPROACH_SPEED = 85;
    public static int DIAMOND_DA62_DESCENT_VERTICAL_SPEED = 600;
    
}