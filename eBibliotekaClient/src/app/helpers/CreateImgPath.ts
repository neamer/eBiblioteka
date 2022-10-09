import { HttpConfig } from '../configs/HttpConfig';

export const createImgPath = (serverPath: string) => {
  if (serverPath) {
    return HttpConfig.staticPath + serverPath;
  }

  return '';
};
