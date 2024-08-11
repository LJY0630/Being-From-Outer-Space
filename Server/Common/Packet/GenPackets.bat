START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
XCOPY /Y GenPackets.cs "../../CapstoneProject/Assets/Resources/Scripts/Packet"
XCOPY /Y GenPackets.cs "../../Server/Packet"

XCOPY /Y ClientPacketManager.cs "../../CapstoneProject/Assets/Resources/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../Server/Packet"