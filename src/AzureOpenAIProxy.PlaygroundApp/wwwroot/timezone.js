export function getBrowserTimeZone() {
    const options = Intl.DateTimeFormat().resolvedOptions();
    return options.timeZone;
}