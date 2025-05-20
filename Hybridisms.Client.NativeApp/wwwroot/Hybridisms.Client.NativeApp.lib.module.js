
export function beforeStart(options, extensions) {
    window.Hybridisms = window.Hybridisms || {};
    window.Hybridisms.services = window.Hybridisms.services || {};

    window.Hybridisms.setService = function (name, service) {
        console.info(`Setting the ${name} Hybridisms service...`);

        window.Hybridisms.services[name] = service;
    }
}

export function afterStarted(blazor) {
}
