console.log('Hybridisms.Shared.lib.module.js');

const pageScriptInfoBySrc = new Map();

function registerPageScriptElement(src) {
    console.log('Hybridisms.Shared.lib.module.js: registerPageScriptElement');

    if (!src) {
        throw new Error('Must provide a non-empty value for the "src" attribute.');
    }

    let pageScriptInfo = pageScriptInfoBySrc.get(src);

    if (pageScriptInfo) {
        pageScriptInfo.referenceCount++;
    } else {
        pageScriptInfo = { referenceCount: 1, module: null };
        pageScriptInfoBySrc.set(src, pageScriptInfo);
        initializePageScriptModule(src, pageScriptInfo);
    }
}

function unregisterPageScriptElement(src) {
    console.log('Hybridisms.Shared.lib.module.js: unregisterPageScriptElement');

    if (!src) {
        return;
    }

    const pageScriptInfo = pageScriptInfoBySrc.get(src);

    if (!pageScriptInfo) {
        return;
    }

    pageScriptInfo.referenceCount--;
}

async function initializePageScriptModule(src, pageScriptInfo) {
    console.info('Hybridisms.Shared.lib.module.js: initializePageScriptModule');

    if (src.startsWith("./")) {
        src = new URL(src.substr(2), document.baseURI).toString();
    }

    const module = await import(src);

    if (pageScriptInfo.referenceCount <= 0) {
        return;
    }

    pageScriptInfo.module = module;
    module.onLoad?.();
    module.onUpdate?.();
}

function onPageLoad() {
    console.info('Hybridisms.Shared.lib.module.js: onPageLoad');

    for (const [src, { module, referenceCount }] of pageScriptInfoBySrc) {
        if (referenceCount <= 0) {
            module?.onDispose?.();
            pageScriptInfoBySrc.delete(src);
        }
    }

    for (const { module } of pageScriptInfoBySrc.values()) {
        module?.onUpdate?.();
    }
}

function onAppStarted(blazor) {
    console.info('Hybridisms.Shared.lib.module.js: onAppStarted');

    customElements.define('page-script', class extends HTMLElement {
        static observedAttributes = ['src'];

        attributeChangedCallback(name, oldValue, newValue) {
            if (name !== 'src') {
                return;
            }

            this.src = newValue;
            unregisterPageScriptElement(oldValue);
            registerPageScriptElement(newValue);
        }

        disconnectedCallback() {
            unregisterPageScriptElement(this.src);
        }
    });
}

export function afterWebStarted(blazor) {
    console.info('Hybridisms.Shared.lib.module.js: afterWebStarted');

    onAppStarted(blazor);

    blazor.addEventListener('enhancedload', onPageLoad);
}

export function afterStarted(blazor) {
    console.info('Hybridisms.Shared.lib.module.js: afterStarted');

    onAppStarted(blazor);

    onPageLoad();
}
