using System.Text.RegularExpressions;
using TonSdk.Core.Boc;

namespace Gwards.Api.Utils;

public static class TonUtil
{
    public static bool IsValidTonAddress(string address)
    {
        return IsAddressEncoded(address) || IsAddressRaw(address);
    }
    
    private static bool IsAddressEncoded(object address) {
        return address is string str && str.Length == 48 && (str.isBase64() || str.isBase64url());
    }

    private static bool IsAddressRaw(object address) {
        const string pattern = "^-?[0-9]:[a-zA-Z0-9]{64}$";
        return address is string str && Regex.IsMatch(str, pattern);
    }
}