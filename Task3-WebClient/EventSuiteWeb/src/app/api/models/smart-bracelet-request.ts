/* tslint:disable */
/* eslint-disable */
import { SmartBraceletStatus } from '../models/smart-bracelet-status';
export interface SmartBraceletRequest {
  accessLatitude1?: number | null;
  accessLatitude2?: number | null;
  accessLongitude1?: number | null;
  accessLongitude2?: number | null;
  currentLatitude?: number | null;
  currentLongitude?: number | null;
  endUsageDate?: Date;
  serialNumber?: string | null;
  startUsageDate?: Date;
  status?: SmartBraceletStatus;
}
