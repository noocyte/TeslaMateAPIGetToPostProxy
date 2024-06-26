﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace TeslamateAPIGetProxy;

public class TeslaAPIResponse
{
    public StatusResponse response { get; set; }
}

public class StatusResponse
{
    public long id { get; set; }
    public int user_id { get; set; }
    public int vehicle_id { get; set; }
    public string vin { get; set; }
    public string display_name { get; set; }
    public string option_codes { get; set; }
    public object color { get; set; }
    public string[] tokens { get; set; }
    public string state { get; set; }
    public bool in_service { get; set; }
    public string id_s { get; set; }
    public bool calendar_enabled { get; set; }
    public int api_version { get; set; }
    public object backseat_token { get; set; }
    public object backseat_token_updated_at { get; set; }
}