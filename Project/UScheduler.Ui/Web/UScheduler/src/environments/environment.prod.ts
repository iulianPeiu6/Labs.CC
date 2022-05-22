import authConfig from "../../auth-config.json"

export const environment = {
  production: true,
  apiEndpoint: "https://uscheduler-api-gateway.azurewebsites.net",
  auth: {
    domain: authConfig.domain,
    clientId: authConfig.clientId,
    redirectUri: window.location.origin,
    audience: authConfig.audience
  }
};
