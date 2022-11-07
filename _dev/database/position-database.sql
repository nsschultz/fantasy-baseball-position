--
-- PostgreSQL database dump
--

-- Dumped from database version 14.1
-- Dumped by pg_dump version 14.2

-- Started on 2022-11-07 19:47:25 UTC

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 211 (class 1259 OID 24586)
-- Name: AdditionalPositions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AdditionalPositions" (
    "ParentCode" character varying(4) NOT NULL,
    "ChildCode" character varying(4) NOT NULL
);


ALTER TABLE public."AdditionalPositions" OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 24581)
-- Name: Positions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Positions" (
    "Code" character varying(4) NOT NULL,
    "FullName" character varying(20),
    "PlayerType" integer NOT NULL,
    "SortOrder" integer NOT NULL
);


ALTER TABLE public."Positions" OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 24576)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 3324 (class 0 OID 24586)
-- Dependencies: 211
-- Data for Name: AdditionalPositions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('1B', 'CIF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('1B', 'IF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('1B', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('2B', 'IF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('2B', 'MIF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('2B', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('3B', 'CIF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('3B', 'IF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('3B', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('C', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('CF', 'OF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('CF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('CIF', 'IF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('CIF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('DH', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('IF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('LF', 'OF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('LF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('MIF', 'IF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('MIF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('OF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('RF', 'OF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('RF', 'UTIL');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('RP', 'P');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('SP', 'P');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('SS', 'IF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('SS', 'MIF');
INSERT INTO public."AdditionalPositions" ("ParentCode", "ChildCode") VALUES ('SS', 'UTIL');


--
-- TOC entry 3323 (class 0 OID 24581)
-- Dependencies: 210
-- Data for Name: Positions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('', 'Unknown', 0, 2147483647);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('1B', 'First Baseman', 1, 1);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('2B', 'Second Baseman', 1, 2);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('3B', 'Third Baseman', 1, 3);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('C', 'Catcher', 1, 0);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('CF', 'Center Feilder', 1, 9);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('CIF', 'Corner Infielder', 1, 5);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('DH', 'Designated Hitter', 1, 12);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('IF', 'Infielder', 1, 7);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('LF', 'Left Fielder', 1, 8);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('MIF', 'Middle Infielder', 1, 6);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('OF', 'Outfielder', 1, 11);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('P', 'Pitcher', 2, 102);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('RF', 'Right Fielder', 1, 10);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('RP', 'Relief Pitcher', 2, 101);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('SP', 'Starting Pitcher', 2, 100);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('SS', 'Shortstop', 1, 4);
INSERT INTO public."Positions" ("Code", "FullName", "PlayerType", "SortOrder") VALUES ('UTIL', 'Utility', 1, 13);


--
-- TOC entry 3322 (class 0 OID 24576)
-- Dependencies: 209
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20220606043342_InitialCreate', '6.0.5');


--
-- TOC entry 3179 (class 2606 OID 24590)
-- Name: AdditionalPositions AdditionalPosition_PK; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AdditionalPositions"
    ADD CONSTRAINT "AdditionalPosition_PK" PRIMARY KEY ("ParentCode", "ChildCode");


--
-- TOC entry 3174 (class 2606 OID 24580)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3177 (class 2606 OID 24585)
-- Name: Positions Position_PK; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Positions"
    ADD CONSTRAINT "Position_PK" PRIMARY KEY ("Code");


--
-- TOC entry 3180 (class 1259 OID 24601)
-- Name: IX_AdditionalPositions_ChildCode; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AdditionalPositions_ChildCode" ON public."AdditionalPositions" USING btree ("ChildCode");


--
-- TOC entry 3175 (class 1259 OID 24602)
-- Name: IX_Positions_SortOrder; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Positions_SortOrder" ON public."Positions" USING btree ("SortOrder");


--
-- TOC entry 3181 (class 2606 OID 24591)
-- Name: AdditionalPositions AdditionalPosition_ChildPosition_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AdditionalPositions"
    ADD CONSTRAINT "AdditionalPosition_ChildPosition_FK" FOREIGN KEY ("ChildCode") REFERENCES public."Positions"("Code") ON DELETE CASCADE;


--
-- TOC entry 3182 (class 2606 OID 24596)
-- Name: AdditionalPositions AdditionalPosition_ParentPosition_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AdditionalPositions"
    ADD CONSTRAINT "AdditionalPosition_ParentPosition_FK" FOREIGN KEY ("ParentCode") REFERENCES public."Positions"("Code") ON DELETE CASCADE;


-- Completed on 2022-11-07 19:47:25 UTC

--
-- PostgreSQL database dump complete
--

