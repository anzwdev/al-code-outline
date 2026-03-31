import { LSPIProjectPlatformCapabilities } from "./lspiProjectPlatformCapabilities";
import { LSPIProjectProfileAffixes } from "./lspiProjectProfileAffixes";
import { LSPIProjectProfileProperties } from "./lspiProjectProfileProperties";

export interface LSPIProjectProfile {
    affixes?: LSPIProjectProfileAffixes;
    properties?: LSPIProjectProfileProperties;
    platformCapabilities?: LSPIProjectPlatformCapabilities;
}