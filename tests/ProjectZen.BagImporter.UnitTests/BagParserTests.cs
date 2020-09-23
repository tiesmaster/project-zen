
using System.IO;
using System.Linq;
using System.Xml;

using FluentAssertions;

using NodaTime;

using Tiesmaster.ProjectZen.Domain.Bag;

using Xunit;

namespace Tiesmaster.ProjectZen.BagImporter.UnitTests
{
    public class BagParserTests
    {
        [Fact]
        public void ParsePanden()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Pand>
          <bag_LVC:identificatie>0599100000661089</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:pandGeometrie>
            <gml:Polygon srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:exterior>
                <gml:LinearRing>
                  <gml:posList srsDimension=""3"" count=""71"">91810.39 435310.8 0.0 91811.73 435310.76 0.0 91811.741 435311.15 0.0 91811.75 435311.47 0.0 91811.691 435312.149 0.0 91811.522 435312.809 0.0 91811.248 435313.433 0.0 91810.876 435314.003 0.0 91810.416 435314.506 0.0 91809.88 435314.927 0.0 91809.283 435315.256 0.0 91808.641 435315.482 0.0 91807.97 435315.602 0.0 91807.61 435315.62 0.0 91807.63 435314.27 0.0 91798.96 435314.3 0.0 91798.98 435314.97 0.0 91798.794 435315.83 0.0 91798.645 435316.101 0.0 91798.457 435316.346 0.0 91798.235 435316.56 0.0 91797.983 435316.739 0.0 91797.707 435316.878 0.0 91797.414 435316.974 0.0 91797.109 435317.026 0.0 91796.801 435317.031 0.0 91796.494 435316.99 0.0 91796.198 435316.904 0.0 91795.918 435316.774 0.0 91795.66 435316.604 0.0 91795.431 435316.398 0.0 91795.235 435316.159 0.0 91795.076 435315.894 0.0 91794.96 435315.608 0.0 91794.887 435315.308 0.0 91794.86 435315.0 0.0 91794.88 435311.78 0.0 91794.57 435311.781 0.0 91790.84 435311.79 0.0 91790.61 435311.61 0.0 91790.62 435311.17 0.0 91787.789 435311.161 0.0 91787.708 435311.155 0.0 91787.629 435311.142 0.0 91787.551 435311.121 0.0 91787.475 435311.094 0.0 91787.401 435311.061 0.0 91787.331 435311.021 0.0 91787.265 435310.975 0.0 91787.203 435310.923 0.0 91787.146 435310.866 0.0 91787.095 435310.804 0.0 91787.049 435310.738 0.0 91787.009 435310.668 0.0 91786.975 435310.595 0.0 91786.948 435310.519 0.0 91786.928 435310.441 0.0 91786.915 435310.361 0.0 91786.909 435310.281 0.0 91786.91 435310.2 0.0 91786.89 435302.91 0.0 91787.122 435302.27 0.0 91787.88 435301.96 0.0 91790.6 435301.94 0.0 91790.8 435301.32 0.0 91792.29 435301.313 0.0 91809.15 435301.23 0.0 91809.301 435301.841 0.0 91809.37 435302.12 0.0 91810.38 435302.1 0.0 91810.39 435310.8 0.0</gml:posList>
                </gml:LinearRing>
              </gml:exterior>
            </gml:Polygon>
          </bag_LVC:pandGeometrie>
          <bag_LVC:bouwjaar>1942</bag_LVC:bouwjaar>
          <bag_LVC:pandstatus>Pand in gebruik</bag_LVC:pandstatus>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2015100900000000</bagtype:begindatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>20151009</bagtype:documentdatum>
            <bagtype:documentnummer>Corsanr.15/54428</bagtype:documentnummer>
          </bag_LVC:bron>
        </bag_LVC:Pand>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(2015, 10, 09, 00, 00);
            var expectedPand = new BagPand(
                "0599100000661089",
                new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, Instant.MaxValue)),
                1942);

            // act
            var panden = BagParser.ParsePanden(xmlReader);

            // assert
            panden.Should().ContainSingle();
            panden.Single().Should().BeEquivalentTo(expectedPand);
        }

        [Fact]
        public void ParsePanden_EinddatumBug()
        {
            // arrange
            var xmlReader = XmlReader.Create(new StringReader(
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<xb:BAG-Extract-Deelbestand-LVC xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:xb=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901"" xmlns:bag_LVC=""http://www.kadaster.nl/schemas/imbag/lvc/v20090901"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:bagtype=""http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901"" xmlns:nen5825=""http://www.kadaster.nl/schemas/imbag/nen5825/v20090901"" xmlns:product_LVC=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901"" xmlns:selecties-extract=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901"" xsi:schemaLocation=""http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901 http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901/BagvsExtractDeelbestandExtractLvc-1.4.xsd"">
  <xb:antwoord>
    <xb:vraag>
      <selecties-extract:Gebied-Registratief>
        <selecties-extract:Gebied-NLD>
          <selecties-extract:GebiedIdentificatie>9999</selecties-extract:GebiedIdentificatie>
          <selecties-extract:GebiedNaam>Nederland</selecties-extract:GebiedNaam>
          <selecties-extract:gebiedTypeNederland>1</selecties-extract:gebiedTypeNederland>
        </selecties-extract:Gebied-NLD>
      </selecties-extract:Gebied-Registratief>
      <selecties-extract:StandTechnischeDatum>20200808</selecties-extract:StandTechnischeDatum>
    </xb:vraag>
    <xb:producten>
      <product_LVC:LVC-product>
        <bag_LVC:Pand>
          <bag_LVC:identificatie>0599100000661089</bag_LVC:identificatie>
          <bag_LVC:aanduidingRecordInactief>N</bag_LVC:aanduidingRecordInactief>
          <bag_LVC:aanduidingRecordCorrectie>0</bag_LVC:aanduidingRecordCorrectie>
          <bag_LVC:officieel>N</bag_LVC:officieel>
          <bag_LVC:pandGeometrie>
            <gml:Polygon srsName=""urn:ogc:def:crs:EPSG::28992"">
              <gml:exterior>
                <gml:LinearRing>
                  <gml:posList srsDimension=""3"" count=""71"">91810.39 435310.8 0.0 91811.73 435310.76 0.0 91811.741 435311.15 0.0 91811.75 435311.47 0.0 91811.691 435312.149 0.0 91811.522 435312.809 0.0 91811.248 435313.433 0.0 91810.876 435314.003 0.0 91810.416 435314.506 0.0 91809.88 435314.927 0.0 91809.283 435315.256 0.0 91808.641 435315.482 0.0 91807.97 435315.602 0.0 91807.61 435315.62 0.0 91807.63 435314.27 0.0 91798.96 435314.3 0.0 91798.98 435314.97 0.0 91798.794 435315.83 0.0 91798.645 435316.101 0.0 91798.457 435316.346 0.0 91798.235 435316.56 0.0 91797.983 435316.739 0.0 91797.707 435316.878 0.0 91797.414 435316.974 0.0 91797.109 435317.026 0.0 91796.801 435317.031 0.0 91796.494 435316.99 0.0 91796.198 435316.904 0.0 91795.918 435316.774 0.0 91795.66 435316.604 0.0 91795.431 435316.398 0.0 91795.235 435316.159 0.0 91795.076 435315.894 0.0 91794.96 435315.608 0.0 91794.887 435315.308 0.0 91794.86 435315.0 0.0 91794.88 435311.78 0.0 91794.57 435311.781 0.0 91790.84 435311.79 0.0 91790.61 435311.61 0.0 91790.62 435311.17 0.0 91787.789 435311.161 0.0 91787.708 435311.155 0.0 91787.629 435311.142 0.0 91787.551 435311.121 0.0 91787.475 435311.094 0.0 91787.401 435311.061 0.0 91787.331 435311.021 0.0 91787.265 435310.975 0.0 91787.203 435310.923 0.0 91787.146 435310.866 0.0 91787.095 435310.804 0.0 91787.049 435310.738 0.0 91787.009 435310.668 0.0 91786.975 435310.595 0.0 91786.948 435310.519 0.0 91786.928 435310.441 0.0 91786.915 435310.361 0.0 91786.909 435310.281 0.0 91786.91 435310.2 0.0 91786.89 435302.91 0.0 91787.122 435302.27 0.0 91787.88 435301.96 0.0 91790.6 435301.94 0.0 91790.8 435301.32 0.0 91792.29 435301.313 0.0 91809.15 435301.23 0.0 91809.301 435301.841 0.0 91809.37 435302.12 0.0 91810.38 435302.1 0.0 91810.39 435310.8 0.0</gml:posList>
                </gml:LinearRing>
              </gml:exterior>
            </gml:Polygon>
          </bag_LVC:pandGeometrie>
          <bag_LVC:bouwjaar>1942</bag_LVC:bouwjaar>
          <bag_LVC:pandstatus>Pand in gebruik</bag_LVC:pandstatus>
          <bag_LVC:tijdvakgeldigheid>
            <bagtype:begindatumTijdvakGeldigheid>2015100900000000</bagtype:begindatumTijdvakGeldigheid>
            <bagtype:einddatumTijdvakGeldigheid>2016100900000000</bagtype:einddatumTijdvakGeldigheid>
          </bag_LVC:tijdvakgeldigheid>
          <bag_LVC:inOnderzoek>N</bag_LVC:inOnderzoek>
          <bag_LVC:bron>
            <bagtype:documentdatum>20151009</bagtype:documentdatum>
            <bagtype:documentnummer>Corsanr.15/54428</bagtype:documentnummer>
          </bag_LVC:bron>
        </bag_LVC:Pand>
      </product_LVC:LVC-product>
    </xb:producten>
  </xb:antwoord>
</xb:BAG-Extract-Deelbestand-LVC>
"));

            var startInstant = Instant.FromUtc(2015, 10, 09, 00, 00);
            var endInstant = Instant.FromUtc(2016, 10, 09, 00, 00);
            var expectedPand = new BagPand(
                "0599100000661089",
                new BagVersion(
                    active: true,
                    correctionIndex: 0,
                    new Interval(startInstant, endInstant)),
                1942);

            // act
            var panden = BagParser.ParsePanden(xmlReader);

            // assert
            panden.Should().ContainSingle();
            panden.Single().Should().BeEquivalentTo(expectedPand);
        }
    }
}